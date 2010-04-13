This is a fork of Sandcastle project.

ResolveReferenceLinksComponent2 class is changed and subclasses can
provide their own TargetId resolution. This is used to resolve Java types
references to Javadoc URLs.

Class Target and its derivatives were changed so other assemblies can
create instances of them and set their values.


This fork is based on the Sandcastle revision 40669.
https://Sandcastle.svn.codeplex.com/svn