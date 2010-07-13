
=== TransformComponent-r39782.patch ===

Patch file that adds XmlEnvVarResolver to TransformComponent.cs.
This resolves is required for XSL files in VS 2010 template
to work correctly (<xsl:import> and <xsl:include> refers to
URLs with %DXROOT% variable).

You can apply this patch to Sandcastle May 2009 release (r39782).

TransformComponent.cs is stored at
    Development/Source/BuildAssembler/BuildComponents/