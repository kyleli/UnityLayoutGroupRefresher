using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Executes the LayoutGroups in reverse order to ensure proper rendering.
/// When multiple nested LayoutGroups need updating, the default system doesn't always 
/// refresh them in the correct hierarchical order. It is important to rebuild the layout from the 
/// bottom up, since the parent objects will need the size of its children.
/// This should be used in scenarios where you have nested LayoutGroups and should solve the issue
/// where when you enable/disable a LayoutGroup it fails to work on the first time you toggle the state.
/// Refer to:
/// https://discussions.unity.com/t/layoutgroup-does-not-refresh-in-its-current-frame/656699/7
/// https://discussions.unity.com/t/content-size-fitter-refresh-problem/678806/18
/// </summary>
public class LayoutGroupRefresher
{
	private readonly List<LayoutGroup> _layoutGroups = new();
	private RectTransform _cachedRoot;
	private bool _isStructureCached;

	public void CacheStructure(RectTransform root)
	{
		if (root == null) return;

		_cachedRoot = root;
		_layoutGroups.Clear();

		// Cache layout groups and depths in a single pass
		Queue<RectTransform> queue = new();
		queue.Enqueue(root);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			var layoutGroup = current.GetComponent<LayoutGroup>();

			if (layoutGroup != null)
			{
				_layoutGroups.Add(layoutGroup);
			}

			// Enqueue children for further processing
			foreach (RectTransform child in current)
			{
				queue.Enqueue(child);
			}
		}

		_isStructureCached = true;
	}

	public void RefreshLayoutGroups(RectTransform root = null)
	{
		if (root != null && root != _cachedRoot)
		{
			CacheStructure(root);
		}

		if (!_isStructureCached) return;

		for (int i = _layoutGroups.Count - 1; i >= 0; i--)
		{
			var group = _layoutGroups[i];
			if (group.transform is RectTransform rectTransform)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
		}
		
		// Force rebuild the root to ensure changes propagate.
		LayoutRebuilder.ForceRebuildLayoutImmediate(root);
	}
}
