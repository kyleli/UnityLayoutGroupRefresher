Have you ever had issues in unity when using multiple LayoutGroups and LayoutRebuilder.ForceRebuildLayoutImmediate() just isn't working? You have to toggle the changes you make twice for it to work, or your UI spends a few frames rearranging itself?

<sup>Yuck, the layout group is all broken with the ContentSizeFitter</sup><br/>
<img src="https://i.imgur.com/r4Rh0Kp.gif"/>

Look no further. You've run into the issue [many](https://discussions.unity.com/t/layoutgroup-does-not-refresh-in-its-current-frame/656699) and I mean [many](https://discussions.unity.com/t/content-size-fitter-refresh-problem/678806) people have. Well here's a solution.
# Unity Layout Group Refresher
<sup>Notice how it doesn't break?</sup><br/>
<img src="https://i.imgur.com/ESMUp8b.gif"/>

This is a code snippet to properly execute the LayoutGroups in reverse order to ensure proper rendering. When multiple nested LayoutGroups need updating, the default system doesn't always refresh them in the correct hierarchical order. It is important to rebuild the layout from the bottom up, since the parent objects will need the size of its children.

This should be used in scenarios where you have nested LayoutGroups and should solve the issue where when you enable/disable a LayoutGroup it fails to work on the first time you toggle the state.

How To Use:
- Add LayoutGroupRefresher.cs to your project file
- Create a new or modify an existing script that handles states and add a new LayoutGroupRefresher class.
- Whenever you make a significant change to the children, e.g. toggling a group of children on or off, call LayoutGroupRefresher.RefreshLayoutGroups().

```c#
private LayoutGroupRefresher _layoutGroupRefresher = new();
private RectTransform _rootRectTransform;

public void ShowGroup()
{
  // Do actions to cause changes to LayoutGroup
  LayoutGroupRefresher.RefreshLayoutGroups(_rootRectTransform);
}

public void HideGroup()
{
  // Do actions to cause changes to LayoutGroup
  LayoutGroupRefresher.RefreshLayoutGroups(_rootRectTransform);
}
```

Features:
- Basic Local Caching
- Simple execution similar to calling LayoutRebuilder.ForceRebuildLayoutImmediate()

Limitations:
- If major changes are made to the structure after caching, the cache will need to be invalidated.
- This has been relatively optimized, but still calls LayoutRebuilder.ForceRebuildLayoutImmediate() on every RectTransform child to the root which can be expensive with extremely complex UIs.

Refer to the following Unity Discussions for more details:
- https://discussions.unity.com/t/layoutgroup-does-not-refresh-in-its-current-frame/656699/7
- https://discussions.unity.com/t/content-size-fitter-refresh-problem/678806/18

MIT License
