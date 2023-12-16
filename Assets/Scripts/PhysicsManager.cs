
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Mathematics;

public class PhysicsManager : MonoBehaviour
{
    public Ball[] balls;
    public Border border;
    public Flipper[] flippers;

    public MyRigidbody[] rigidbodies;

    public static PhysicsManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Quick way to get all ball and border components
        // Doesn't allow balls/borders to be added at runtime

        RefreshRigidbodies();

        //balls = FindObjectsOfType<Ball>();
        //border = FindObjectOfType<Border>();
        //flippers = FindObjectsOfType<Flipper>();

    }

    public void RefreshRigidbodies()
    {
        rigidbodies = FindObjectsOfType<MyRigidbody>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].Simulate();
            MyCollider collider = rigidbodies[i].GetCollider();
            if (collider == null) continue;
            for (int j = i + 1; j < rigidbodies.Length; j++)
            {
                MyCollider otherCollider = rigidbodies[j].GetCollider();
                if (otherCollider != null)
                {
                    //TODO: Broad Phase: Instead of having just a list, make spatial aware data structure
                    HandleCollision(collider, otherCollider);
                }
            }
        }

        return;
    }

    // This is taken from the tutorial
    // https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/04-pinball.html
    Vector2 ClosestPointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = ab.sqrMagnitude;
        if (t == 0)
            return a;
        t = Mathf.Max(0, Mathf.Min(1, (Vector2.Dot(p, ab) - Vector2.Dot(a, ab)) / t));
        return a + ab * t;
    }


    void HandleCollision(MyCollider collider1, MyCollider collider2)
    {
        if ((collider1 as dynamic).Collides(collider2 as dynamic))
        {

            //Static colliders dont move anyways
            if (collider1.GetRigidbody().isStatic && collider2.GetRigidbody().isStatic)
                return;

            //One is nonStatic
            if (collider1.GetRigidbody().isStatic && !collider2.GetRigidbody().isStatic)
            {
                //Swap col1 and col2 to not duplicate code
                MyCollider temp = collider1;
                collider1 = collider2;
                collider2 = temp;
            }

            if (!collider1.GetRigidbody().isStatic && collider2.GetRigidbody().isStatic)
            {

                (Vector2, Vector2, Vector2, float) penetration = (collider1 as dynamic).Penetrate(collider2 as dynamic);
                //This is how much they overlap, going from col2 to col1
                Vector2 normal = penetration.Item3.normalized;

                if (false)
                {
                    Debug.Log(penetration.Item1 + ", " + penetration.Item2 + ", " + penetration.Item3 + ", " + penetration.Item4);
                }
                Vector2 displacement = normal * penetration.Item4;
                collider1.transform.position += new Vector3(displacement.x, displacement.y, 0);

                Vector2 vel1 = collider1.myRigidbody.PointVelocity(penetration.Item1);
                Vector2 vel2 = collider2.myRigidbody.PointVelocity(penetration.Item2);

                Vector2 relVel = vel1 - vel2;

                float normalVel = Vector2.Dot(relVel, normal);

                //Debug.Log("Normalvel: " + normalVel);

                float bounciness;
                if (collider1.myRigidbody.overrideBounciness)
                    bounciness = collider1.myRigidbody.bounciness;
                if (collider2.myRigidbody.overrideBounciness)
                    bounciness = collider2.myRigidbody.bounciness;
                else bounciness = Math.Min(collider1.myRigidbody.bounciness, collider2.myRigidbody.bounciness);


                float jTop = -(1 + bounciness) * normalVel;

                Vector3 rap = new Vector3(penetration.Item1.x, penetration.Item1.y, 0) - collider1.transform.position;
                float rCrossNSquared = Vector3.Cross(rap, new Vector3(normal.x, normal.y, 0)).z;
                rCrossNSquared = rCrossNSquared * rCrossNSquared;

                float j = jTop / ((1 / collider1.myRigidbody.GetMass()) + (rCrossNSquared / collider1.myRigidbody.momentOfInertia));

                //Debug.Log("J: " + j + ", Normal: " + normal + ", Point: " + penetration.Item1 + normal * penetration.Item4);
                collider1.myRigidbody.AddImpulse(j * normal, penetration.Item1);
                /*(Vector2, Vector2, Vector2, float) penetration = (collider1 as dynamic).Penetrate(collider2 as dynamic);

                //Vector of correction
                Vector2 normal = penetration.Item3;
                float dist = penetration.Item4;
                //Where the force should be applied
                Vector2 attackPoint = penetration.Item1;

                //Point allready moving away
                if(Vector2.Dot(normal, collider1.myRigidbody.PointVelocity(attackPoint)) > 0)
                    return;

                collider1.myRigidbody.position += normal * dist;

                //Adjust velocity: direction of correction vector
                float adjustmentVelocityStrength = Math.Abs(Vector2.Dot(normal, collider1.myRigidbody.velocity));
                adjustmentVelocityStrength *= (1 + Math.Min(collider1.myRigidbody.bounciness, collider2.myRigidbody.bounciness));

                collider1.myRigidbody.AddVelocity(normal * adjustmentVelocityStrength, attackPoint);*/
            }

            else
            {


                (Vector2, Vector2, Vector2, float) penetration = (collider1 as dynamic).Penetrate(collider2 as dynamic);
                //This is how much they overlap, going from col2 to col1
                Vector2 normal = penetration.Item3.normalized;

                //Displace rigidbodies according to weight (lighter get more displacement)
                float m1 = collider1.myRigidbody.GetMass(); float m2 = collider2.myRigidbody.GetMass();
                float massCoefficient1 = m1 / (m1 + m2);
                float massCoefficient2 = 1 - massCoefficient1;

                Vector2 displacement1 = massCoefficient2 * normal * penetration.Item4;
                Vector2 displacement2 = massCoefficient1 * normal * penetration.Item4;

                collider1.transform.position += new Vector3(displacement1.x, displacement1.y, 0);
                collider2.transform.position -= new Vector3(displacement2.x, displacement2.y, 0);

                Vector2 vel1 = collider1.myRigidbody.PointVelocity(penetration.Item1);
                Vector2 vel2 = collider2.myRigidbody.PointVelocity(penetration.Item2);

                Vector2 relVel = vel1 - vel2;

                float normalVel = Vector2.Dot(relVel, normal);

                float bounciness;
                if (collider1.myRigidbody.overrideBounciness)
                    bounciness = collider1.myRigidbody.bounciness;
                if (collider2.myRigidbody.overrideBounciness)
                    bounciness = collider2.myRigidbody.bounciness;
                else bounciness = Math.Min(collider1.myRigidbody.bounciness, collider2.myRigidbody.bounciness);

                float jTop = -(1 + bounciness) * normalVel;

                Vector3 rap = new Vector3(penetration.Item1.x, penetration.Item1.y, 0) - collider1.transform.position;
                Vector3 rbp = new Vector3(penetration.Item2.x, penetration.Item2.y, 0) - collider2.transform.position;
                float raCrossNSquared = Vector3.Cross(rap, new Vector3(normal.x, normal.y, 0)).z;
                raCrossNSquared = raCrossNSquared * raCrossNSquared;
                float rbCrossNSquared = Vector3.Cross(rbp, new Vector3(normal.x, normal.y, 0)).z;
                rbCrossNSquared = rbCrossNSquared * rbCrossNSquared;

                float j = jTop / ((1 / collider1.myRigidbody.GetMass()) + (1 / collider2.myRigidbody.GetMass())
                + (raCrossNSquared / collider1.myRigidbody.momentOfInertia) + (rbCrossNSquared / collider2.myRigidbody.momentOfInertia));

                collider1.myRigidbody.AddImpulse(j * normal, penetration.Item1);
                collider2.myRigidbody.AddImpulse(-j * normal, penetration.Item2);


            }
        }

    }


    //Project vector a onto b
    public Vector2 ProjectVector(Vector2 a, Vector2 b)
    {
        b.Normalize();
        return Vector2.Dot(a, b) * b;
    }

    public float Cross2D(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (a.y * b.x);
    }
}