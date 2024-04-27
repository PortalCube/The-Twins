using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;

[System.Serializable]
public class EntityAnimation {
    public enum AnimationType {
        Translate,
        Rotate
    }

    public enum AnimationAxis {
        X,
        Y,
        Z
    }

    public AnimationType type = AnimationType.Translate;
    public AnimationAxis axis = AnimationAxis.X;
    public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    // None: 한번만 실행
    // Loop: 무한 반복
    // PingPong: 왕복 반복
    public Tween.LoopType loopType = Tween.LoopType.None;

    public float startValue = 0f;
    public float endValue = 1f;

    public float duration = 1f; // 애니메이션의 플레이 시간 (1 cycle, PingPong에서는 1/2 cycle의 시간)
    public float pingPongDelay = 0f; // PingPong에서, Ping과 Pong 사이의 대기 시간
    public float delay = 0f; // Loop 혹은 PingPong에서, cycle과 cycle 사이의 대기 시간
    public float offset = 0f; // 첫 시작시 애니메이션 진행률, 0 ~ 1

    public TweenBase tween;
}