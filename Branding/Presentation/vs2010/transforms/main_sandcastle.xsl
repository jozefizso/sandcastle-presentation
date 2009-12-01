<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1">

  <!-- stuff specified to comments authored in DDUEXML -->

  <xsl:include href="html_body_header.xsl"/>
  <xsl:include href="html_body_navigation.xsl"/>
  <xsl:include href="html_body.xsl"/>
  <xsl:include href="html_body_footer.xsl"/>
  
  <xsl:include href="utilities_reference.xsl" />

  <xsl:variable name="summary" select="normalize-space(/document/comments/summary)" />
  <xsl:variable name="abstractSummary" select="/document/comments/summary" />
  <xsl:variable name="hasSeeAlsoSection" select="boolean((count(/document/comments//seealso | /document/reference/elements/element/overloads//seealso) > 0)  or 
                           ($group='type' or $group='member' or $group='list'))"/>
  <xsl:variable name="examplesSection" select="boolean(string-length(/document/comments/example[normalize-space(.)]) > 0)"/>
  <xsl:variable name="languageFilterSection" select="boolean(string-length(/document/comments/example[normalize-space(.)]) > 0)" />

  <xsl:template name="getParameterDescription">
    <xsl:param name="name" />
    <xsl:apply-templates select="/document/comments/param[@name=$name]" />
  </xsl:template>

  <xsl:template name="getReturnsDescription">
    <xsl:param name="name" />
    <xsl:apply-templates select="/document/comments/param[@name=$name]" />
  </xsl:template>

  <xsl:template name="getElementDescription">
    <xsl:apply-templates select="summary[1]" />
  </xsl:template>

  <xsl:template name="getOverloadSummary">
    <xsl:apply-templates select="overloads" mode="summary"/>
  </xsl:template>

  <xsl:template name="getOverloadSections">
    <xsl:apply-templates select="overloads" mode="sections"/>
  </xsl:template>

  <xsl:template name="getInternalOnlyDescription">

  </xsl:template>


  <!-- block sections -->

  <xsl:template match="summary">
    <div class="summary">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="overloads" mode="summary">
    <xsl:choose>
      <xsl:when test="count(summary) > 0">
        <xsl:apply-templates select="summary" />
      </xsl:when>
      <xsl:otherwise>
        <div class="summary">
          <xsl:apply-templates/>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="overloads" mode="sections">
    <xsl:apply-templates select="remarks" />
    <xsl:apply-templates select="example"/>
  </xsl:template>

  <xsl:template match="value">
    <xsl:call-template name="subSection">
      <xsl:with-param name="title">
        <include item="fieldValueTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:apply-templates />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="returns">
    <xsl:call-template name="subSection">
      <xsl:with-param name="title">
        <include item="methodValueTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:apply-templates />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="templates">
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'templates'" />
      <xsl:with-param name="title">
        <include item="templatesTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <dl>
          <xsl:for-each select="template">
            <xsl:variable name="templateName" select="@name" />
            <dt>
              <span class="parameter">
                <xsl:value-of select="$templateName"/>
              </span>
            </dt>
            <dd>
              <xsl:apply-templates select="/document/comments/typeparam[@name=$templateName]" />
            </dd>
          </xsl:for-each>
        </dl>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="remarks">
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'remarks'"/>
      <xsl:with-param name="title">
        <include item="remarksTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:apply-templates />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="example">
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'example'"/>
      <xsl:with-param name="title">
        <include item="examplesTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:apply-templates />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="para">
    <p>
      <xsl:apply-templates />
    </p>
  </xsl:template>

  <xsl:template match="code">

    <xsl:variable name="codeLang">
      <xsl:choose>
        <xsl:when test="@language = 'vbs'">
          <xsl:text>VBScript</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'vb' or @language = 'vb#'  or @language = 'VB'" >
          <xsl:text>VisualBasic</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'c#' or @language = 'cs' or @language = 'C#'" >
          <xsl:text>CSharp</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'cpp' or @language = 'cpp#' or @language = 'c' or @language = 'c++' or @language = 'C++'" >
          <xsl:text>ManagedCPlusPlus</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'j#' or @language = 'jsharp'">
          <xsl:text>JSharp</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'js' or @language = 'jscript#' or @language = 'jscript' or @language = 'JScript'">
          <xsl:text>JScript</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'xml'">
          <xsl:text>xmlLang</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'html'">
          <xsl:text>html</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'vb-c#'">
          <xsl:text>visualbasicANDcsharp</xsl:text>
        </xsl:when>
        <xsl:when test="@language = 'xaml' or @language = 'XAML'">
          <xsl:text>XAML</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>other</xsl:text>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:call-template name="codeSection">
      <xsl:with-param name="codeLang" select="$codeLang" />
    </xsl:call-template>

  </xsl:template>

  <xsl:template name="exceptions">
    <xsl:if test="count(/document/comments/exception) &gt; 0">
      <xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'exceptions'"/>
        <xsl:with-param name="title">
          <include item="exceptionsTitle" />
        </xsl:with-param>
        <xsl:with-param name="content">
          <div class="tableSection">
            <table width="100%" cellspacing="2" cellpadding="5" frame="lhs" >
              <tr>
                <th class="exceptionNameColumn">
                  <include item="exceptionNameHeader" />
                </th>
                <th class="exceptionConditionColumn">
                  <include item="exceptionConditionHeader" />
                </th>
              </tr>
              <xsl:for-each select="/document/comments/exception">
                <tr>
                  <td>
                    <referenceLink target="{@cref}" qualified="true" />
                  </td>
                  <td>
                    <xsl:apply-templates select="." />
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="permissions">
    <xsl:if test="count(/document/comments/permission) &gt; 0">
      <xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'permissions'" />
        <xsl:with-param name="title">
          <include item="permissionsTitle" />
        </xsl:with-param>
        <xsl:with-param name="content">
          <div class="tableSection">
            <table width="100%" cellspacing="2" cellpadding="5" frame="lhs" >
              <tr>
                <th class="permissionNameColumn">
                  <include item="permissionNameHeader" />
                </th>
                <th class="permissionDescriptionColumn">
                  <include item="permissionDescriptionHeader" />
                </th>
              </tr>
              <xsl:for-each select="/document/comments/permission">
                <tr>
                  <td>
                    <referenceLink target="{@cref}" qualified="true" />
                  </td>
                  <td>
                    <xsl:apply-templates select="." />
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="seealso">
    <xsl:if test="$hasSeeAlsoSection">
      <xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'seeAlso'" />
        <xsl:with-param name="title">
          <include item="relatedTitle" />
        </xsl:with-param>
        <xsl:with-param name="content">
          <xsl:call-template name="autogenSeeAlsoLinks"/>
          <xsl:for-each select="/document/comments//seealso | /document/reference/elements/element/overloads//seealso">
            <div class="seeAlsoStyle">
              <xsl:apply-templates select=".">
                <xsl:with-param name="displaySeeAlso" select="true()" />
              </xsl:apply-templates>
            </div>
          </xsl:for-each>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="list[@type='bullet']">
    <ul>
      <xsl:for-each select="item">
        <li>
          <xsl:apply-templates />
        </li>
      </xsl:for-each>
    </ul>
  </xsl:template>

  <xsl:template match="list[@type='number']">
    <ol>
      <xsl:for-each select="item">
        <li>
          <xsl:apply-templates />
        </li>
      </xsl:for-each>
    </ol>
  </xsl:template>

  <xsl:template match="list[@type='table']">
    <div class="tableSection">
      <table width="100%" cellspacing="2" cellpadding="5" frame="lhs" >
        <xsl:for-each select="listheader">
          <tr>
            <xsl:for-each select="*">
              <th>
                <xsl:apply-templates />
              </th>
            </xsl:for-each>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="item">
          <tr>
            <xsl:for-each select="*">
              <td>
                <xsl:apply-templates />
              </td>
            </xsl:for-each>
          </tr>
        </xsl:for-each>
      </table>
    </div>
  </xsl:template>

  <!-- inline tags -->

  <xsl:template match="see[@cref]">
    <xsl:choose>
      <xsl:when test="normalize-space(.)">
        <referenceLink target="{@cref}">
          <xsl:value-of select="." />
        </referenceLink>
      </xsl:when>
      <xsl:otherwise>
        <referenceLink target="{@cref}"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="see[@href]">
    <xsl:choose>
      <xsl:when test="normalize-space(.)">
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="@href"/>
          </xsl:attribute>
          <xsl:value-of select="." />
        </a>
      </xsl:when>
      <xsl:otherwise>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="@href"/>
          </xsl:attribute>
          <xsl:value-of select="@href" />
        </a>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="seealso[@href]">
    <xsl:param name="displaySeeAlso" select="false()" />
    <xsl:if test="$displaySeeAlso">
      <xsl:choose>
        <xsl:when test="normalize-space(.)">
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="@href"/>
            </xsl:attribute>
            <xsl:value-of select="." />
          </a>
        </xsl:when>
        <xsl:otherwise>
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="@href"/>
            </xsl:attribute>
            <xsl:value-of select="@href" />
          </a>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template match="see[@langword]">
    <span class="keyword">
      <xsl:choose>
        <xsl:when test="@langword='null' or @langword='Nothing' or @langword='nullptr'">
          <span class="languageSpecificText">
            <span class="cs">null</span>
            <span class="vb">Nothing</span>
            <span class="cpp">nullptr</span>
          </span>
        </xsl:when>
        <xsl:when test="@langword='static' or @langword='Shared'">
          <span class="languageSpecificText">
            <span class="cs">static</span>
            <span class="vb">Shared</span>
            <span class="cpp">static</span>
          </span>
        </xsl:when>
        <xsl:when test="@langword='virtual' or @langword='Overridable'">
          <span class="languageSpecificText">
            <span class="cs">virtual</span>
            <span class="vb">Overridable</span>
            <span class="cpp">virtual</span>
          </span>
        </xsl:when>
        <xsl:when test="@langword='true' or @langword='True'">
          <span class="languageSpecificText">
            <span class="cs">true</span>
            <span class="vb">True</span>
            <span class="cpp">true</span>
          </span>
        </xsl:when>
        <xsl:when test="@langword='false' or @langword='False'">
          <span class="languageSpecificText">
            <span class="cs">false</span>
            <span class="vb">False</span>
            <span class="cpp">false</span>
          </span>
        </xsl:when>
        <xsl:when test="@langword='abstract'">
          <span class="languageSpecificText">
            <span class="cs">abstract</span>
            <span class="vb">MustInherit</span>
            <span class="cpp">abstract</span>
          </span>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@langword" />
        </xsl:otherwise>
      </xsl:choose>
    </span>
    <xsl:choose>
      <xsl:when test="@langword='null' or @langword='Nothing' or @langword='nullptr'">
        <span class="nu">
          <include item="nullKeyword"/>
        </span>
      </xsl:when>
      <xsl:when test="@langword='static' or @langword='Shared'">
        <span class="nu">
          <include item="staticKeyword"/>
        </span>
      </xsl:when>
      <xsl:when test="@langword='virtual' or @langword='Overridable'">
        <span class="nu">
          <include item="virtualKeyword"/>
        </span>
      </xsl:when>
      <xsl:when test="@langword='true' or @langword='True'">
        <span class="nu">
          <include item="trueKeyword"/>
        </span>
      </xsl:when>
      <xsl:when test="@langword='false' or @langword='False'">
        <span class="nu">
          <include item="falseKeyword"/>
        </span>
      </xsl:when>
      <xsl:when test="@langword='abstract'">
        <span class="nu">
          <include item="abstractKeyword"/>
        </span>
      </xsl:when>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="seealso">
    <xsl:param name="displaySeeAlso" select="false()" />
    <xsl:if test="$displaySeeAlso">
      <xsl:choose>
        <xsl:when test="normalize-space(.)">
          <referenceLink target="{@cref}" qualified="true">
            <xsl:value-of select="." />
          </referenceLink>
        </xsl:when>
        <xsl:otherwise>
          <referenceLink target="{@cref}" qualified="true" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template match="c">
    <span class="code">
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match="paramref">
    <span class="parameter">
      <xsl:value-of select="@name" />
    </span>
  </xsl:template>

  <xsl:template match="typeparamref">
    <span class="typeparameter">
      <xsl:value-of select="@name" />
    </span>
  </xsl:template>

  <xsl:template match="syntax">
    <xsl:if test="count(*) > 0">
      <xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'syntax'" />
        <xsl:with-param name="title">
          <include item="syntaxTitle"/>
        </xsl:with-param>
        <xsl:with-param name="content">
          <xsl:call-template name="syntaxBlocks" />
          <!-- parameters & return value -->
          <xsl:apply-templates select="/document/reference/parameters" />
          <xsl:apply-templates select="/document/reference/templates" />
          <xsl:apply-templates select="/document/comments/value" />
          <xsl:apply-templates select="/document/comments/returns" />
          <xsl:apply-templates select="/document/reference/implements" />
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="runningHeader">
    <include item="runningHeaderText" />
  </xsl:template>

  <!-- pass through html tags -->

  <xsl:template match="p|ol|ul|li|dl|dt|dd|table|tr|th|td|a|img|b|i|strong|em|del|sub|sup|br|hr|h1|h2|h3|h4|h5|h6|pre|div|span|blockquote|abbr|acronym|u|font|map|area">
    <xsl:copy>
      <xsl:copy-of select="@*" />
      <xsl:apply-templates />
    </xsl:copy>
  </xsl:template>

  <!-- extra tag support -->

  <xsl:template match="threadsafety">
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'threadSafety'" />
      <xsl:with-param name="title">
        <include item="threadSafetyTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:choose>
          <xsl:when test="normalize-space(.)">
            <xsl:apply-templates />
          </xsl:when>
          <xsl:otherwise>
            <xsl:if test="@static='true'">
              <include item="staticThreadSafe" />
            </xsl:if>
            <xsl:if test="@static='false'">
              <include item="staticNotThreadSafe" />
            </xsl:if>
            <xsl:if test="@instance='true'">
              <include item="instanceThreadSafe" />
            </xsl:if>
            <xsl:if test="@instance='false'">
              <include item="instanceNotThreadSafe" />
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="note">
    <div class="alert">
      <img>
        <includeAttribute item="iconPath" name="src">
          <parameter>alert_note.gif</parameter>
        </includeAttribute>
        <includeAttribute name="title" item="noteAltText" />
      </img>
      <xsl:text> </xsl:text>
      <include item="noteTitle" />
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="preliminary">
    <div class="preliminary">
      <include item="preliminaryText" />
    </div>
  </xsl:template>

  <!-- move these off into a shared file -->

  <xsl:template name="createReferenceLink">
    <xsl:param name="id" />
    <xsl:param name="qualified" select="false()" />

    <referenceLink target="{$id}" qualified="{$qualified}" />

  </xsl:template>

  <xsl:template name="section">
    <xsl:param name="toggleSwitch" />
    <xsl:param name="title" />
    <xsl:param name="content" />

    <!--<xsl:variable name="toggleTitle" select="concat($toggleSwitch,'Toggle')" />-->
    <xsl:variable name="toggleSection" select="concat($toggleSwitch,'Section')" />

    <div class="regionArea">
      <h2 class="regiontitle">
        <xsl:copy-of select="$title" />
      </h2>
      <div class='hrdiv'><hr class='regionhr' /></div>
    </div>

    <a id="{$toggleSection}"><xsl:comment/></a>
    <xsl:copy-of select="$content" />
  </xsl:template>

  <xsl:template name="subSection">
    <xsl:param name="title" />
    <xsl:param name="content" />

    <h4 class="subHeading">
      <xsl:copy-of select="$title" />
    </h4>
    <xsl:copy-of select="$content" />

  </xsl:template>

  <xsl:template name="memberIntro">
    <xsl:if test="$subgroup='members'">
      <p>
        <xsl:apply-templates select="/document/reference/containers/summary"/>
      </p>
    </xsl:if>
    <xsl:call-template name="memberIntroBoilerplate"/>
  </xsl:template>

  <xsl:template name="codelangAttributes">
    <xsl:call-template name="mshelpCodelangAttributes">
      <xsl:with-param name="snippets" select="/document/comments/example/code" />
    </xsl:call-template>
  </xsl:template>

</xsl:stylesheet>
