// Copyright � Microsoft Corporation.
// This source file is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using Microsoft.Ddue.Tools.CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace DBCSFix
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConsoleApplication.WriteBanner();

            // get and validate args
            OptionCollection programOptions = new OptionCollection();
            programOptions.Add(new SwitchOption("?", "Show this help page."));
            programOptions.Add(new StringOption("d", @"The directory containing CHM input files (e.g., HHP file). For example, 'C:\DocProject\Output\Chm'. Default is the current directory."));
            programOptions.Add(new StringOption("l", @"The language code ID in decimal. For example, '1033'. Default is '1033' (for EN-US)."));
            ParseArgumentsResult options = programOptions.ParseArguments(args);
            if (options.Options["?"].IsPresent)
                programOptions.WriteOptionSummary(Console.Error);

            // determine the working dir
            string chmDirectory;
            if (options.Options["d"].IsPresent)
                chmDirectory = options.Options["d"].Value.ToString();
            else
                chmDirectory = Environment.CurrentDirectory;

            // determine the desired language
            string lcid;
            if (options.Options["l"].IsPresent)
                lcid = options.Options["l"].Value.ToString();
            else
                lcid = "1033";

            // ensure working dir exists
            if (!Directory.Exists(chmDirectory))
            {
                Console.WriteLine("The specified directory '{0}' doesn't exist. Quitting.", chmDirectory);
                return;
            }

            // convert unsupported high-order chars to ascii equivalents
            substituteAsciiEquivalents(chmDirectory, lcid);

            // no further work required for 1033
            if (String.Equals(lcid, "1033"))
                return;

            // convert unsupported chars to named entities
            substituteNamedEntities(chmDirectory);

            // convert charset declarations from utf8 to proper ansi codepage value
            substituteCodepages(chmDirectory, lcid);

            // convert char encodings from utf8 to ansi
            convertUtf8ToAnsi(chmDirectory, lcid);
        }

        private static void convertUtf8ToAnsi(string chmDirectory, string lcid)
        {
            Console.WriteLine("Converting character encodings from utf8 to ansi.");
            Encoding ansi = Encoding.GetEncoding(encodingNameForLcid(lcid));

            List < string > files = new List < string >();
            files.AddRange(Directory.GetFiles(chmDirectory, "*.htm", SearchOption.AllDirectories));

            foreach (string file in files)
            {
                using (StreamWriter sw = new StreamWriter(file + ".tmp", false, ansi))
                {
                    using (StreamReader input = new StreamReader(file))
                    {
                        Encoding sourceEncoding = input.CurrentEncoding;
                        string line;
                        while ((line = input.ReadLine()) != null)
                        {
                            byte[] sourceBytes = sourceEncoding.GetBytes(line);
                            byte[] ansiBytes = Encoding.Convert(sourceEncoding, ansi, sourceBytes);
                            sw.WriteLine(ansi.GetString(ansiBytes));
                        }
                    }
                }

                File.Delete(file);
                File.Move(file + ".tmp", file);
            }
        }

        private static string encodingNameForLcid(string lcid)
        {
            string charset = System.Configuration.ConfigurationSettings.AppSettings[lcid];
            if (String.IsNullOrEmpty(charset))
                return "Windows-1252";
            else
                return charset;
        }

        private static void substituteAsciiEquivalents(string chmDirectory, string lcid)
        {
            Console.WriteLine("Converting unsupported high-order characters to 7-bit ASCII equivalents.");

            /* substitution table:
             * Char name                    utf8 (hex)          ascii
             * Non-breaking space	    	\xC2\xA0		    "&nbsp;" (for all languages except Japanese)
             * Non-breaking hyphen	    	\xE2\x80\x91		"-"
             * En dash				        \xE2\x80\x93		"-"
             * Left curly single quote	    \xE2\x80\x98		"'"
             * Right curly single quote 	\xE2\x80\x99		"'"
             * Left curly double quote	    \xE2\x80\x9C		"\""
             * Right curly double quote 	\xE2\x80\x9D		"\""
             * Horizontal ellipsis          U+2026              "..."
             */

            Dictionary < Regex, string > substitutionPatterns = new Dictionary < Regex, string >();
            substitutionPatterns.Add(new Regex(@"\u2018|\u2019", RegexOptions.Compiled), "'");
            substitutionPatterns.Add(new Regex(@"\u201C|\u201D", RegexOptions.Compiled), "\"");
            substitutionPatterns.Add(new Regex(@"\u2026", RegexOptions.Compiled), "...");
            if (chmDirectory != "1041")
                substitutionPatterns.Add(new Regex(@"\u00A0", RegexOptions.Compiled), "&nbsp;");
            else
                substitutionPatterns.Add(new Regex(@"\u00A0", RegexOptions.Compiled), " ");

            string ansi = Encoding.GetEncoding(encodingNameForLcid(lcid)).HeaderName;
            Console.WriteLine("EncodingName: " + ansi);
            if (!string.Equals(ansi, "Windows-1252"))
            {
                substitutionPatterns.Add(new Regex(@"\u2011|\u2013", RegexOptions.Compiled), "-");
                substituteInFiles(chmDirectory, "*.htm", substitutionPatterns);
            }
            else
            {
                // replace em-dashes with hyphens, if not windows-1252 (e.g., 1033)
                substitutionPatterns.Add(new Regex(@"\u2011|\u2013|\u2014", RegexOptions.Compiled), "-");
            }
        }

        private static void substituteCodepages(string chmDirectory, string lcid)
        {
            Console.WriteLine("Inserting charset declarations.");

            Dictionary < Regex, string > substitutionPatterns = new Dictionary < Regex, string >();
            substitutionPatterns.Add(new Regex(@"CHARSET=UTF-8", RegexOptions.Compiled | RegexOptions.IgnoreCase), "CHARSET=" + encodingNameForLcid(lcid));

            substituteInFiles(chmDirectory, "*.htm", substitutionPatterns);
        }

        private static void substituteInFiles(string directory, string fileSpec, ICollection < KeyValuePair < Regex, string > > substitutionPatterns)
        {
            Debug.Assert(Directory.Exists(directory), "Specified directory doesn't exist.");
            Debug.Assert(!String.IsNullOrEmpty(fileSpec), "FileSpec is empty");
            Debug.Assert(substitutionPatterns.Count > 0, "No substitution patterns.");

            string[] files = Directory.GetFiles(directory, fileSpec, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                using (StreamWriter output = new StreamWriter(file + ".tmp", true, Encoding.UTF8))
                {
                    using (StreamReader input = new StreamReader(file))
                    {
                        string line;
                        while ((line = input.ReadLine()) != null)
                        {
                            foreach (KeyValuePair < Regex, string > pattern in substitutionPatterns)
                            {
                                line = pattern.Key.Replace(line, pattern.Value);
                            }
                            output.WriteLine(line);
                        }
                    }
                }

                File.Delete(file);
                File.Move(file + ".tmp", file);
            }
        }

        private static void substituteNamedEntities(string chmDirectory)
        {
            Console.WriteLine("Converting other unsupported high-order characters to named entities.");

            /* substitution table:
             * Char name                    utf8 (hex)          named entity
             * Copyright	            	\xC2\xA0		    &copy
             * Registered trademark        	\xC2\xAE		    &reg
             * Em dash  	            	\xE2\x80\x94		&mdash;
             * Trademark		            \xE2\x84\xA2		&trade;
             */

            Dictionary < Regex, string > substitutionPatterns = new Dictionary < Regex, string >();
            substitutionPatterns.Add(new Regex(@"\u00A9", RegexOptions.Compiled), "&copy;");
            substitutionPatterns.Add(new Regex(@"\u00AE", RegexOptions.Compiled), "&reg;");
            substitutionPatterns.Add(new Regex(@"\u2014", RegexOptions.Compiled), "&mdash;");
            substitutionPatterns.Add(new Regex(@"\u2122", RegexOptions.Compiled), "&trade;");

            substituteInFiles(chmDirectory, "*.htm", substitutionPatterns);
        }
    }
}
