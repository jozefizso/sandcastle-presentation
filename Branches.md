# Branches #

Description of branches in the **sandcastle-presentation** project.


## Sandcastle\_June2010\_Merge ##

This branch is used for merging and integrating [Sandcastle June 2010 release](http://sandcastle.codeplex.com/releases/view/47665).


`hg clone https://sandcastle-presentation.googlecode.com/hg/#Sandcastle_June2010_Merge sp-june2010`


## Sandcastle\_July2009\_Merge ##

This branch is intented to contain changes made to vs2005 presentation style. _Better name for this branch would be **Sandcastle\_vs2005\_updates** as it is based on source code of the **vs2005** folder and contains changes made to files in this folder in consequent Sandcastle releases._

[Revision 0](http://code.google.com/p/sandcastle-presentation/source/detail?r=431797f46db65c6d31bcb1f50863f140f898fa09) contains files from the vs2005 presentation folder ([Sandcastle revision 21990](http://sandcastle.codeplex.com/SourceControl/changeset/changes/21990)). Other changesets contain new versions of the vs2005 folder from [Sancastle repository](https://sandcastle.svn.codeplex.com/svn/Development/Presentation/vs2005). This changes are then merged into main development branch.

`hg clone https://sandcastle-presentation.googlecode.com/hg/#Sandcastle_July2009_Merge sp-july2009`


## SHFB\_updates ##

This branch is used to track changes between Sandcastle Help File Builder releases and integrate thouse changes from **SHFB\_updates** branch to the **default** development branch.

`hg clone https://sandcastle-presentation.googlecode.com/hg/#SHFB_updates sp-shfb_updates`