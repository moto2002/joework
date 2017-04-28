namespace MsgPack.Serialization
{
    using System;
    using System.Diagnostics;

    internal static class Tracer
    {
        public static readonly TraceSource Emit = new TraceSource("MsgPack.Serialization.Emit");

        public static class EventId
        {
            public const int DefineType = 0x66;
            public const int ILTrace = 0x65;
            public const int MultipleAccessorFound = 0x386;
            public const int NoAccessorFound = 0x385;
            public const int ReadOnlyValueTypeMember = 0x387;
            public const int UnsupportedType = 0x2a95;
        }

        public static class EventType
        {
            public const TraceEventType DefineType = TraceEventType.Verbose;
            public const TraceEventType ILTrace = TraceEventType.Verbose;
            public const TraceEventType MultipleAccessorFound = TraceEventType.Verbose;
            public const TraceEventType NoAccessorFound = TraceEventType.Verbose;
            public const TraceEventType ReadOnlyValueTypeMember = TraceEventType.Verbose;
            public const TraceEventType UnsupportedType = TraceEventType.Information;
        }
    }
}

