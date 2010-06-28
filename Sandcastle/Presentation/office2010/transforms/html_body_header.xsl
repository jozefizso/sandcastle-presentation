<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1"
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
>
  <!--<xsl:import href="../../vs2010/transforms/html_body_navigation.xsl"/>-->
  <xsl:import href="html_body_navigation.xsl"/>

  <xsl:template name="bodyHeader">
    <div class="header">
      <xsl:call-template name="navBreadcrumbs" />

      <div class="headerContent">
        <xsl:call-template name="projectTitle" />

        <xsl:call-template name="searchBox" />
      </div>
    </div>
  </xsl:template>
  
</xsl:stylesheet>
