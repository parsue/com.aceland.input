using System;

namespace AceLand.Input.State
{
    public struct InputData<T> where T : struct
    {
        public string Name { get; internal set; }
        public T RawData { get; internal set; }
        public T ProvidedData { get; internal set; }
        public T LastRawData { get; internal set; }
        public T LastProvidedData { get; internal set; }
        public float Time { get; internal set; }
        public Func<T, T> ProvidedMethod { get; internal set; }
    }
}