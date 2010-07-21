<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1"
        xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
        xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
        xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
>

  <xsl:template name="navBreadcrumbs">
    <div class="breadcrumbs">
      <conceptualLink target="{/document/toc/topics/topic[1]/@id}" class="headerTab">Home</conceptualLink>
      &gt;
      <a href="#">Another page</a>
    </div>
  </xsl:template>

  <xsl:template name="navigation">
    <!-- empty sidebar navigation -->
  </xsl:template>
  
</xsl:stylesheet>
