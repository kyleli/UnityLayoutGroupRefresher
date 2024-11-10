# UnityLayoutGroupRefresher
Code snippet to properly execute the LayoutGroups in reverse order to ensure proper rendering. When multiple nested LayoutGroups need updating, the default system doesn't always refresh them in the correct hierarchical order. It is important to rebuild the layout from the bottom up, since the parent objects will need the size of its children.
