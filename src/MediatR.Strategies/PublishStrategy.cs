namespace MediatR.Strategies;

public enum PublishStrategy
{
    SyncContinueOnException = 0,

    SyncStopOnException = 1,

    Async = 2,

    ParallelNoWait = 3,

    ParallelWhenAll = 4,

    ParallelWhenAny = 5,
}
