using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class EntityAnimationController : MonoBehaviour {

    public EntityAnimation[] animations;

    // Start is called before the first frame update
    void Start() {
        StartAnimation();
    }

    // Update is called once per frame
    void Update() {

    }

    // Entity 애니메이션 시작
    public void StartAnimation() {
        foreach (var animation in animations) {
            Action<float> onValueCallback = GetValueCallback(animation);

            // PingPong 루프의 경우, Ping 부분만 재생
            bool isPingPong = animation.loopType == Tween.LoopType.PingPong;
            float delay = animation.delay - animation.offset;

            Tween.Value(animation.startValue, animation.endValue, onValueCallback, animation.duration, delay, animation.curve, Tween.LoopType.None, null, () => OnAnimationComplete(animation, isPingPong));
        }
    }

    // Tween.Value에 전달할 OnValue 콜백 함수를 만들어서 반환
    // OnValue 콜백 함수는, EntityAnimation의 type과 axis에 따라, transform의 localPosition 혹은 localRotation을 변경
    Action<float> GetValueCallback(EntityAnimation animation) {
        return (value) => {
            switch (animation.type) {
                case EntityAnimation.AnimationType.Translate:
                    transform.localPosition = GetValueVector(animation.axis, transform.localPosition, value);
                    break;
                case EntityAnimation.AnimationType.Rotate:
                    transform.localRotation = Quaternion.Euler(GetValueVector(animation.axis, transform.localRotation.eulerAngles, value));
                    break;
            }
        };
    }

    // axis와 value에 따라서 original 벡터를 수정하여 반환하는 함수
    Vector3 GetValueVector(EntityAnimation.AnimationAxis axis, Vector3 original, float value) {
        // C# 8.0, Switch expression
        return axis switch {
            EntityAnimation.AnimationAxis.X => new Vector3(value, original.y, original.z),
            EntityAnimation.AnimationAxis.Y => new Vector3(original.x, value, original.z),
            EntityAnimation.AnimationAxis.Z => new Vector3(original.x, original.y, value),
            _ => original,
        };
    }

    // Tween 애니메이션이 종료될 때마다 실행
    void OnAnimationComplete(EntityAnimation animation, bool isPingEnded = false) {
        float start = animation.startValue;
        float end = animation.endValue;
        float delay = animation.delay;
        bool isPing = false;

        // 루프가 None인 경우, 애니메이션을 종료
        if (animation.loopType == Tween.LoopType.None) {
            return;
        }

        if (animation.loopType == Tween.LoopType.PingPong) {
            // PingPong 루프에서
            if (isPingEnded) {
                // Ping 부분이 완료되었을 때, start와 end가 바뀐 Pong 부분을 재생
                start = animation.endValue;
                end = animation.startValue;
                delay = animation.pingPongDelay;
            } else {
                // Pong 부분이 완료되었을 때, Ping 부분을 재생
                isPing = true;
            }
        }

        // Loop 혹은 PingPong 루프 애니메이션 진행
        Tween.Value(start, end, GetValueCallback(animation), animation.duration, delay, animation.curve, Tween.LoopType.None, null, () => OnAnimationComplete(animation, isPing));
    }
}
