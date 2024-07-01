using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Over Event Channel", menuName = "Events/Game Over Event Channel")]
public class GameOverEventChannel : GenericEventChannelSO<GameOverEvent> {
}
[System.Serializable]
public struct GameOverEvent {}
