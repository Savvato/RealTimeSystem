using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ObjectModel.Object
{
    /// <summary>
    /// Дифференциальное уравнение зависимости температуры уходящего газа (T(t)) от объема поступающего воздуха (G(t))
    /// 136.177 T''(t) + 82.713 T'(t) + T(t) = 0.822 G(t)
    /// </summary>
    public class ObjectValueCalculator
    {
        private ObjectHandler Handler { get; }

        /// <summary>
        /// Период расчета
        /// </summary>
        public double H { get; set; } = 0.001;

        /// <summary>
        /// Температура газа
        /// </summary>
        public double T { get; set; } = 0;

        /// <summary>
        /// Объем поступающего воздуха
        /// </summary>
        public double G { get; set; } = 0;

        /// <summary>
        /// Подставное T
        /// </summary>
        public double T1 { get; set; } = 0;
        
        public ObjectValueCalculator(ObjectHandler handler)
        {
            Handler = handler;
        }

        public void Run()
        {
            while (true)
            {
                for (int i = 0; i < 100; i++)
                {
                    Calculate();
                    Thread.Sleep(1);
                }
                Handler.SendValue();
            }
        }

        private void Calculate()
        {
            double t1 = T1 + H * ((0.822 * G - 82.713 * T1 - T) / 136.177);
            double t = T + H * T1;

            T1 = t1;
            T = t;
        }
    }
}
