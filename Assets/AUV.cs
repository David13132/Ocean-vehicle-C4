using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUV : MonoBehaviour
{
    [Header("推进参数")]
    public float forwardForce = 5f;    // W/S 前后推力
    public float upForce = 4f;         // R/F 上下推力
    public float turnTorque = 2f;      // A/D 转向力矩

    [Header("水下动力学参数")]
    public float buoyancyForce = 9.8f; // 向上浮力
    public float dragLinear = 1.2f;    // 平动水阻力
    public float dragAngular = 0.8f;   // 旋转水阻力

    private Rigidbody rb;

    void Start()
    {
        // 自动获取刚体，不用手动拖rb框
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 1. 竖直浮力
        rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);

        // 2. 读取按键
        float forwardInput = Input.GetAxis("Vertical"); // W=1 S=-1
        float turnInput = Input.GetAxis("Horizontal");  // D=1 A=-1
        float upInput = 0f;
        if (Input.GetKey(KeyCode.R)) upInput = 1f;      // 按R 上浮
        if (Input.GetKey(KeyCode.F)) upInput = -1f;     // 按F 下潜

        // 3. 施加推力
        rb.AddForce(transform.forward * forwardForce * forwardInput); // 前后
        rb.AddForce(transform.up * upForce * upInput);                // 上下
        rb.AddTorque(transform.up * turnTorque * turnInput);          // 转向

        // 4. 水阻力
        rb.velocity *= (1 - dragLinear * Time.fixedDeltaTime);
        rb.angularVelocity *= (1 - dragAngular * Time.fixedDeltaTime);
    }
}

