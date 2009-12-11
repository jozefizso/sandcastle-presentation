<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
         >

  <xsl:import href="html_body.xsl"/>
  
  <xsl:template match="/document">
    <html xmlns:xlink="http://www.w3.org/1999/xlink">
      <head>
        <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
        <title>
          <xsl:call-template name="topicTitlePlain"/>
        </title>
        <xsl:call-template name="insertStylesheets" />
        <!--<xsl:call-template name="insertScripts" />-->
        <xsl:call-template name="insertMetadata" />
      </head>
      <body>
        <xsl:call-template name="bodyHeaderMain"/>
        <xsl:call-template name="main"/>
      </body>
    </html>
  </xsl:template>

  <!-- document head -->

  <xsl:template name="insertStylesheets">
    <!--
    <link rel="stylesheet" type="text/css" href="../styles/msdn10/Msdn10-bn20091118.css" />
    <link rel="stylesheet" type="text/css" href="../styles/msdn10/Msdn10_vstudio-bn20091118.css" />
    <link rel="stylesheet" type="text/css" href="../styles/msdn10/mtps-bn20091118.css" />
    -->
    <link rel="stylesheet" type="text/css" href="../Styles/lightweight.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/lw-code.css" />

    <!-- make mshelp links work -->
    <!--<link rel="stylesheet" type="text/css" href="ms-help://Hx/HxRuntime/HxLink.css" />-->
    <!--<link rel="stylesheet" type="text/css" href="ms-help://Dx/DxRuntime/DxLink.css" />-->
  </xsl:template>

  <xsl:template name="insertScripts">
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath">
        <parameter>EventUtilities.js</parameter>
      </includeAttribute>
      <xsl:text> </xsl:text>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath">
        <parameter>SplitScreen.js</parameter>
      </includeAttribute>
      <xsl:text> </xsl:text>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath">
        <parameter>Dropdown.js</parameter>
      </includeAttribute>
      <xsl:text> </xsl:text>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath">
        <parameter>script_manifold.js</parameter>
      </includeAttribute>
      <xsl:text> </xsl:text>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath">
        <parameter>script_feedBack.js</parameter>
      </includeAttribute>
      <xsl:text> </xsl:text>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath">
        <parameter>CheckboxMenu.js</parameter>
      </includeAttribute>
      <xsl:text> </xsl:text>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath">
        <parameter>CommonUtilities.js</parameter>
      </includeAttribute>
      <xsl:text> </xsl:text>
    </script>

  </xsl:template>

  <xsl:template name="insertMetadata">
    <!-- TODO: generate metadata page... -->
    <xsl:comment>
      <xsl:text>Looking for metadata? Use the robot view instead </xsl:text>
      <xsl:value-of select="concat($key, '_robot.htm')"/>
    </xsl:comment>
  </xsl:template>

  <xsl:template name="insertMetadataOld">
    <xsl:if test="$metadata='true'">
      <xml>
        <!-- mshelp metadata -->

        <!-- insert toctitle -->
        <xsl:if test="normalize-space(/document/metadata/tableOfContentsTitle) and (/document/metadata/tableOfContentsTitle != /document/metadata/title)">
          <MSHelp:TOCTitle Title="{/document/metadata/tableOfContentsTitle}" />
        </xsl:if>

        <!-- link index -->
        <MSHelp:Keyword Index="A" Term="{$key}" />

        <!-- authored K -->
        <xsl:variable name="docset" select="translate(/document/metadata/attribute[@name='DocSet'][1]/text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz ')"/>
        <xsl:for-each select="/document/metadata/keyword[@index='K']">
          <xsl:variable name="nestedKeywordText">
            <xsl:call-template name="nestedKeywordText"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="not(contains(text(),'[')) and ($docset='avalon' or $docset='wpf' or $docset='wcf' or $docset='windowsforms')">
              <MSHelp:Keyword Index="K">
                <includeAttribute name="Term" item="kIndexTermWithTechQualifier">
                  <parameter>
                    <xsl:value-of select="text()"/>
                  </parameter>
                  <parameter>
                    <xsl:value-of select="$docset"/>
                  </parameter>
                  <parameter>
                    <xsl:value-of select="$nestedKeywordText"/>
                  </parameter>
                </includeAttribute>
              </MSHelp:Keyword>
            </xsl:when>
            <xsl:otherwise>
              <MSHelp:Keyword Index="K" Term="{concat(text(),$nestedKeywordText)}" />
            </xsl:otherwise>
          </xsl:choose>
          <!--
        <MSHelp:Keyword Index="K">
          <xsl:choose>
            <xsl:when test="normalize-space($docset)='' or contains(text(),'[')">
              <xsl:attribute name="Term">
                <xsl:value-of select="concat(text(),$nestedKeywordText)"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <includeAttribute name="Term" item="kIndexTermWithTechQualifier">
                <parameter><xsl:value-of select="text()"/></parameter>
                <parameter><xsl:value-of select="$docset"/></parameter>
                <parameter><xsl:value-of select="$nestedKeywordText"/></parameter>
              </includeAttribute>
            </xsl:otherwise>
          </xsl:choose>
        </MSHelp:Keyword>
        -->
        </xsl:for-each>

        <!-- authored S -->
        <xsl:for-each select="/document/metadata/keyword[@index='S']">
          <MSHelp:Keyword Index="S">
            <xsl:attribute name="Term">
              <xsl:value-of select="text()" />
              <xsl:for-each select="keyword[@index='S']">
                <xsl:text>, </xsl:text>
                <xsl:value-of select="text()"/>
              </xsl:for-each>
            </xsl:attribute>
          </MSHelp:Keyword>
          <!-- S index keywords need to be converted to F index keywords -->
          <MSHelp:Keyword Index="F">
            <xsl:attribute name="Term">
              <xsl:value-of select="text()" />
              <xsl:for-each select="keyword[@index='S']">
                <xsl:text>, </xsl:text>
                <xsl:value-of select="text()"/>
              </xsl:for-each>
            </xsl:attribute>
          </MSHelp:Keyword>
        </xsl:for-each>

        <!-- authored F -->
        <xsl:for-each select="/document/metadata/keyword[@index='F']">
          <MSHelp:Keyword Index="F">
            <xsl:attribute name="Term">
              <xsl:value-of select="text()" />
              <xsl:for-each select="keyword[@index='F']">
                <xsl:text>, </xsl:text>
                <xsl:value-of select="text()"/>
              </xsl:for-each>
            </xsl:attribute>
          </MSHelp:Keyword>
        </xsl:for-each>

        <!-- authored B -->
        <xsl:for-each select="/document/metadata/keyword[@index='B']">
          <MSHelp:Keyword Index="B">
            <xsl:attribute name="Term">
              <xsl:value-of select="text()" />
              <xsl:for-each select="keyword[@index='B']">
                <xsl:text>, </xsl:text>
                <xsl:value-of select="text()"/>
              </xsl:for-each>
            </xsl:attribute>
          </MSHelp:Keyword>
        </xsl:for-each>

        <!-- Topic version -->
        <MSHelp:Attr Name="RevisionNumber" Value="{/document/topic/@revisionNumber}" />

        <!-- Asset ID -->
        <MSHelp:Attr Name="AssetID" Value="{/document/topic/@id}" />

        <!-- Abstract -->
        <xsl:variable name="abstract" select="string(/document/topic//ddue:para[1])" />
        <xsl:choose>
          <xsl:when test="string-length($abstract) &gt; 254">
            <MSHelp:Attr Name="Abstract" Value="{concat(substring($abstract,1,250), ' ...')}" />
          </xsl:when>
          <xsl:when test="string-length($abstract) &gt; 0">
            <MSHelp:Attr Name="Abstract" Value="{$abstract}" />
          </xsl:when>
        </xsl:choose>

        <!-- Autogenerate codeLang attributes based on the snippets -->
        <xsl:call-template name="mshelpCodelangAttributes">
          <xsl:with-param name="snippets" select="/document/topic/*//ddue:snippets/ddue:snippet" />
        </xsl:call-template>

        <!-- authored attributes -->
        <xsl:for-each select="/document/metadata/attribute">
          <MSHelp:Attr Name="{@name}" Value="{text()}" />
        </xsl:for-each>

        <!-- TopicType attribute -->
        <xsl:for-each select="/document/topic/*[1]">
          <MSHelp:Attr Name="TopicType">
            <includeAttribute name="Value" item="TT_{local-name()}"/>
          </MSHelp:Attr>
        </xsl:for-each>

        <!-- Locale attribute -->
        <MSHelp:Attr Name="Locale">
          <includeAttribute name="Value" item="locale"/>
        </MSHelp:Attr>

      </xml>
    </xsl:if>
  </xsl:template>
  
  <!-- document body -->
  <xsl:template name="bodyContent">
    <!-- freshness date -->
    <!--<xsl:call-template name="writeFreshnessDate">
      <xsl:with-param name="ChangedHistoryDate" select="/document/topic/*//ddue:section[ddue:title = 'Change History']/ddue:content/ddue:table/ddue:row[1]/ddue:entry[1]"/>
    </xsl:call-template>-->

    <xsl:apply-templates select="topic" />

    <xsl:text> </xsl:text>
    <!-- changed table section -->
    <!--<xsl:call-template name="writeChangedTable" />-->
  </xsl:template>
  
</xsl:stylesheet>