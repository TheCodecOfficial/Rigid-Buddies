using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public Ball[] balls;
    public Border border;
    public Flipper[] flippers;

    void Start()
    {
        // Quick way to get all ball and border components
        // Doesn't allow balls/borders to be added at runtime
        balls = FindObjectsOfType<Ball>();
        border = FindObjectOfType<Border>();
        flippers = FindObjectsOfType<Flipper>();
    }

    void Update()
    {
        //todo añadir colisiones con flippers
        //todo I guess input manager
        for (int i = 0; i < flippers.Length; i++)
        {   
            flippers[i].Simulate();
        }
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].Simulate();
            for (int j = i + 1; j < balls.Length; j++)
            {
                HandleBallBallCollision(balls[i], balls[j]);
            }
            for (var j = 0; j < flippers.Length; j++)
            {
				handleBallFlipperCollision(balls[i], flippers[j]);
            }
            HandleBallBorderCollision(balls[i], border);
        }
    }

    // This is taken from the tutorial
    // https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/04-pinball.html
    void HandleBallBallCollision(Ball ball1, Ball ball2)
    {
        float restitution = Mathf.Min(ball1.restitution, ball2.restitution);
        Vector2 dir = ball2.pos - ball1.pos;
        float d = dir.magnitude;
        if (d == 0.0 || d > ball1.radius + ball2.radius)
            return;

        dir /= d;

        float corr = (ball1.radius + ball2.radius - d) / 2f;
        ball1.pos += dir * -corr;
        ball2.pos += dir * corr;

        float v1 = Vector2.Dot(ball1.vel, dir);
        float v2 = Vector2.Dot(ball2.vel, dir);

        float m1 = ball1.mass;
        float m2 = ball2.mass;

        var newV1 = (m1 * v1 + m2 * v2 - m2 * (v1 - v2) * restitution) / (m1 + m2);
        var newV2 = (m1 * v1 + m2 * v2 - m1 * (v2 - v1) * restitution) / (m1 + m2);

        ball1.vel += dir * (newV1 - v1);
        ball2.vel += dir * (newV2 - v2);
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

    // This is taken from the tutorial
    // https://github.com/matthias-research/pages/blob/master/tenMinutePhysics/04-pinball.html
    void HandleBallBorderCollision(Ball ball, Border border)
    {
        Vector2[] points = border.points;
        if (points.Length < 3)
            return;

        // find closest segment;
        Vector2 d;
        Vector2 closest = new(0, 0);
        Vector2 ab;
        Vector2 normal = new(0, 0);

        float dist, minDist = 0;

        for (var i = 0; i < points.Length; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[(i + 1) % points.Length];
            Vector2 c = ClosestPointOnSegment(ball.pos, a, b);
            d = ball.pos - c;
            dist = d.magnitude;
            if (i == 0 || dist < minDist)
            {
                minDist = dist;
                closest = c;
                ab = b - a;
                normal = Vector2.Perpendicular(ab);
            }
        }

        // push out
        d = ball.pos - closest;
        dist = d.magnitude;
        if (dist == 0.0)
        {
            d = normal;
            dist = normal.magnitude;
        }
        d /= dist;

        if (Vector2.Dot(d, normal) >= 0.0)
        {
            if (dist > ball.radius)
                return;
            ball.pos += d * (ball.radius - dist);
        }
        else
            ball.pos += d * -(dist + ball.radius);

        // update velocity
        float v = Vector2.Dot(ball.vel, d);
        var vnew = Mathf.Abs(v) * ball.restitution;

        ball.vel += d * (vnew - v);
    }
    void handleBallFlipperCollision(Ball ball, Flipper flipper) 
	{
		Vector2 closest = ClosestPointOnSegment(ball.pos, flipper.pos, flipper.getTip());
		Vector2 dir = ball.pos - closest;
		float d = dir.magnitude;
		if (d == 0.0 || d > ball.radius + flipper.radius)
			return;

		dir = dir*(1 / d);

		float corr = ball.radius + flipper.radius - d;
		ball.pos += dir* corr;

		// update velocitiy

		Vector2 radius = closest + dir* flipper.radius - flipper.pos;

		Vector2 surfaceVel = Vector2.Perpendicular(radius);
		surfaceVel *= flipper.currentAngularVelocity;
		float v = Vector2.Dot(ball.vel , dir);
		float vnew = Vector2.Dot(surfaceVel,dir)* ball.restitution* flipper.restitution;

		ball.vel = ball.vel + dir*( vnew - v);
	}

}