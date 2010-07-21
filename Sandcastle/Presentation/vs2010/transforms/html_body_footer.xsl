<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
         >

  <xsl:import href="utilities_reference.xsl"/>
  
  <!-- Footer stuff -->
  <xsl:template name="footer">
    <div class="footer">
      <div id="footer" class="footerContainer cl_footer_slice">
        <div class="footerLogoContainer">
          <div class="footerContent">
            <span class="copyright">
              <include item="copyrightStatement"/>
            </span>
            <span class="pipe">|</span>
            <xsl:call-template name="footer_branding_note" />
          </div>
        </div>
      </div>
    </div>
  </xsl:template>

  <xsl:template name="footer_branding_note">
    Template is based on the MSDN 2010 branding.
  </xsl:template>
  
</xsl:stylesheet>