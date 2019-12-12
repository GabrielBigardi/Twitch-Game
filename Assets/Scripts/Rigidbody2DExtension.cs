﻿using UnityEngine;

//public static class Rigidbody2DExtension
//{
//
//    static float multiplier = 200f;
//
//    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
//    {
//        var dir = (body.transform.position - explosionPosition);
//        float wearoff = 1 - (dir.magnitude / explosionRadius);
//        body.AddForce(dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff);
//    }
//
//    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
//    {
//        var dir = (body.transform.position - explosionPosition);
//        float wearoff = 1 - (dir.magnitude / explosionRadius);
//        Vector3 baseForce = dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff;
//        body.AddForce(baseForce * multiplier);
//
//        float upliftWearoff = 1 - upliftModifier / explosionRadius;
//        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
//        body.AddForce(upliftForce * multiplier);
//    }
//}

public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
        float multiplier = 125f;

        // força horizontal
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        body.AddForce(baseForce * multiplier);

        // força vertical
        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce);
    }
}