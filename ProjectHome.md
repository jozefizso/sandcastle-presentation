This project aims to create new presentation template for [Sandcastle](http://sandcastle.codeplex.com/) that will generate documentation in the style of MSDN Lightweight branding.

Currently I've oriented much more on support for conceptual documention and integration with [SHFB](http://shfb.codeplex.com/).

**Note:** On June 28th I reset repository and pushed new one with edited history which has fixed [revision ef2a32bb66](http://code.google.com/p/sandcastle-presentation/source/detail?r=ef2a32bb660a8cbed241b04ddf18b1636bbfcbe3). The former one deleted vs2010 filed and created new ones without storing this changes as rename operation and this caused issues with merging changes from branch Sandcastle\_July2009\_Merge.

Sample of rendering StoredNumber class from test.cs (part of Sandcastle examples) as of 21. Nov 2009:
<img src='http://wiki.sandcastle-presentation.googlecode.com/hg/Images/Sandcastle-VS2010Branding-StoredNumber_Class.png' width='400'>

<a href='http://wiki.sandcastle-presentation.googlecode.com/hg/Images/Sandcastle-VS2010Branding-StoredNumber_Class.png'>Full image</a>

Sandcastle Presentation project has some Extensions which enhance generated documentation.<br>
<br>
<h3>Support for Javadoc</h3>

(this is only a preliminary support for Javadoc)<br>
<br>
Custom XmlDoclet for Javadoc can generate required doc and reflection XML documents for Java sources:<br>
<img src='http://wiki.sandcastle-presentation.googlecode.com/hg/Images/Sandcastle-javadoc-example-2009-11-28.png' width='400'>

<a href='http://wiki.sandcastle-presentation.googlecode.com/hg/Images/Sandcastle-javadoc-example-2009-11-28.png'>Full image</a>