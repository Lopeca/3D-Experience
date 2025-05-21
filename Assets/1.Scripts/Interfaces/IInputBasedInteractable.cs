using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 탑다운 개인과제에서 밟으면 작동하는 트랩도 상호작용인데 왜 IInteractable이 아닌지 모호하다는 피드백이 있어
// 인터페이스 이름을 조금 장황하게 지어보았습니다.

public interface IInputBasedInteractable
{
    public string Prompt { get; }
    public void OnTargeted();
    public void OnTargetLost();
    public void Interact();
}
