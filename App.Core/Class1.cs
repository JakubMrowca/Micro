using System;

namespace App.Core
{
    public interface IWolneRzarty
    {
        void Rzart();
    }
    public class WolneRzarty:IWolneRzarty
    {
        public void Rzart()
        {
            throw new NotImplementedException();
        }
    }
}
