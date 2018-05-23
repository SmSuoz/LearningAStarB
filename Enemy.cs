using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningAStarB
{
    class Enemy : Agent
    {
        public Enemy(int x, int y) : base(x, y)
        {
            speed = 1.5f;
        }
        public override void Draw()
        {
            sprite.DrawSolidColor(255, 0, 0);
        }
        //public override void Update(float deltaTime)
        //{
        //    base.Update(deltaTime);
        //    if()
        //}

    }
}
