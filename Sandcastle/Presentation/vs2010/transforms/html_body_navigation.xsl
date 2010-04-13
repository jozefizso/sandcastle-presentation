<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
         >

  <xsl:import href="utilities_reference.xsl"/>

  <xsl:template name="navigation">
    <div class="navigation">
      <xsl:call-template name="searchBox" />
      <xsl:call-template name="tableOfContent" />
    </div>
  </xsl:template>


  <xsl:template name="searchBox">
    <div class="searchcontainer">
      <form id="SearchForm" action="" method="get">
        <div class="Search">
          <img class="LT_image cl_lt_search" alt="">
            <includeAttribute name="src" item="imagePath">
              <parameter>030c41d9079671d09a62d8e2c1db6973.gif</parameter>
            </includeAttribute>
          </img>

          <div class="SearchPanel cl_slice_Search">
            <input id="searchTextBox" name="query" type="text"
							maxlength="200" class="SearchBox">
              <includeAttribute name="value" item="searchInputText" />
            </input>

            <a onClick="javascript:document.getElementById('SearchForm').submit();" style="display: inline-block; vertical-align: top;">
              <span class="SearchButton">
                <img class="cl_search_icon" alt="" title="Search">
                  <includeAttribute name="src" item="imagePath">
                    <parameter>msdn2010branding-stripe1.png</parameter>
                  </includeAttribute>
                </img>
              </span>
            </a>
          </div>

          <img class="RT_image cl_rt_search" alt="">
            <includeAttribute name="src" item="imagePath">
              <parameter>030c41d9079671d09a62d8e2c1db6973.gif</parameter>
            </includeAttribute>
          </img>
        </div>
      </form>
    </div>
  </xsl:template>

  <xsl:template name="tableOfContent">
    <div class="navcontainer">
      <div class="nav">
        Table of Content

        <xsl:variable name="currentTopic" select="//toc//tocTopic[@id=/document/topic/@id]" />
        <xsl:variable name="ancestorTopics" select="$currentTopic/ancestor::tocTopic" />
        <xsl:variable name="childTopics" select="$currentTopic/child::tocTopic" />

        <xsl:if test="$ancestorTopics">
          <div class="toclevel0 ancestry">
            <xsl:apply-templates select="$ancestorTopics" mode="conceptual"/>
          </div>
        </xsl:if>
        <xsl:if test="$currentTopic">
          <div class="toclevel1 current">
            <xsl:apply-templates select="$currentTopic" mode="conceptual"/>
          </div>
        </xsl:if>
        <xsl:if test="$childTopics">
          <div class="toclevel2 child">
            <xsl:apply-templates select="$childTopics" mode="conceptual"/>
          </div>
        </xsl:if>

        <xsl:call-template name="relatedTopicsNavigation" />
      </div>
    </div>
  </xsl:template>

  <xsl:template name="relatedTopicsNavigation">
    
  </xsl:template>

  <xsl:template match="tocTopic" mode="conceptual">
    <conceptualLink>
      <xsl:attribute name="target">
        <xsl:value-of select="@id"/>
      </xsl:attribute>
    </conceptualLink>
  </xsl:template>
  
  <!-- links -->

  <xsl:template match="ddue:externalLink" mode="navigation">
    <a>
      <xsl:attribute name="href">
        <xsl:value-of select="ddue:linkUri" />
      </xsl:attribute>
      <xsl:value-of select="ddue:linkText" />
    </a>
    <br />
  </xsl:template>

  <xsl:template match="ddue:link" mode="navigation">
    <xsl:choose>
      <xsl:when test="starts-with(@xlink:href,'#')">
        <!-- in-page link -->
        <a href="{@xlink:href}">
          <xsl:apply-templates />
        </a>
      </xsl:when>
      <xsl:otherwise>
        <!-- verified, external link -->
        <conceptualLink target="{@xlink:href}">
          <xsl:apply-templates />
        </conceptualLink>
      </xsl:otherwise>
    </xsl:choose>
    <br />
  </xsl:template>

  <xsl:template match="ddue:legacyLink" mode="navigation">
    <xsl:choose>
      <xsl:when test="starts-with(@xlink:href,'#')">
        <!-- in-page link -->
        <a href="{@xlink:href}">
          <xsl:apply-templates />
        </a>
      </xsl:when>
      <xsl:otherwise>
        <!-- unverified, external link -->
        <mshelp:link keywords="{@xlink:href}" tabindex="0">
          <xsl:apply-templates />
        </mshelp:link>
      </xsl:otherwise>
    </xsl:choose>
    <br />
  </xsl:template>
  
</xsl:stylesheet>