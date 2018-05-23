using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LearningAStarB
{
    class Agent
    {
        private List<Node> path;
        private Node target;
        protected Sprite sprite;
        public int X { get { return Convert.ToInt32(sprite.position.X); } }
        public int Y { get { return Convert.ToInt32(sprite.position.Y); } }
        protected float speed;

        public Agent(int x, int y)
        {
            sprite = new Sprite(1, 1);
            sprite.position = new Vector2(x, y);
            speed = 3f;
            target = null;
        }

        public void SetPath(List<Node> path)
        {
            this.path = path;
            if (target == null)
            {
                target = path[0];
                path.RemoveAt(0);
            }
        }

        public virtual void Update(float deltaTime)
        {
            if (target != null)
            {
                Vector2 dest = new Vector2(target.X, target.Y);
                Vector2 dir = dest - sprite.position;
                float dist = dir.Length;
                if (dist < 0.01f)
                {
                    sprite.position = dest;
                    if (path.Count > 0)
                    {
                        target = path[0];
                        path.RemoveAt(0);
                    }
                    else
                    {
                        target = null;
                    }
                }
                else
                {
                    sprite.position += dir.Normalized() * deltaTime * speed;
                }
            }
        }

        public virtual void Draw()
        {
            sprite.DrawSolidColor(0f, 0f, 1f);
        }
    }
}
