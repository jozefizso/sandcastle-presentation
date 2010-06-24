<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1"
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
>

  <xsl:import href="html_body_header.xsl"/>
  
  
  <xsl:template name="insertStylesheets">
    <link rel="stylesheet" type="text/css" href="../styles/office2010.css" />
  </xsl:template>

  <xsl:template name="documentMajorTitle">
    <!-- empty document major title -->
  </xsl:template>
  
</xsl:stylesheet>
