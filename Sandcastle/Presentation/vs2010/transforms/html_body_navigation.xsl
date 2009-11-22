<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
         >

  <xsl:import href="utilities_reference.xsl"/>

  <xsl:template name="navigation">
    <div class="OH_leftNav">
      <xsl:call-template name="searchBox" />
      <xsl:call-template name="tableOfContent" />
    </div>
  </xsl:template>


  <xsl:template name="searchBox">
    <div id="div_Search" class="OH_SearchBox">
        <div class="SearchWrapper">
          <img class="lt_image">
            <includeAttribute name="title" item="cautionAltText" />
            <includeAttribute name="src" item="imagePath">
              <parameter>lt_search.gif</parameter>
            </includeAttribute>
          </img>
          
          <div class="SearchPanel">
            <input name="SearchTextBox" type="text"
							maxlength="200" class="SearchBox">
              <includeAttribute name="value" item="searchInputText" />
            </input>
              <a class="button">
                <input type="image" name="SearchImageButton" id="SearchImageButton"
                  class="SearchButton"
                  style="border-width: 0px;">
                  <includeAttribute name="src" item="imagePath">
                    <parameter>searchicon.gif</parameter>
                  </includeAttribute>
                </input>
              </a>
            </div>

          <img class="rt_image">
            <includeAttribute name="title" item="cautionAltText" />
            <includeAttribute name="src" item="imagePath">
              <parameter>rt_search.gif</parameter>
            </includeAttribute>
          </img>
        </div>
    </div>
  </xsl:template>

  <xsl:template name="tableOfContent">
    <div id="toc">
      <div id="toc_parent">
        Table of Content
      </div>
    </div>
  </xsl:template>
  
</xsl:stylesheet>