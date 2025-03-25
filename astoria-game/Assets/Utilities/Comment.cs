using UnityEngine;

/// <summary>
///  A class used for maintaining per-object documentation
/// </summary>
public class Comment : MonoBehaviour
{
    [TextArea(3, 8)]
    [SerializeField] private string Notes;
}
