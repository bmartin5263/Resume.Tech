namespace ResumeTech.Common.Utility;

public static class TaskUtils {
    
    public static Task<O> Cast<O>(this Task<object> task) {
        TaskCompletionSource<O> res = new TaskCompletionSource<O>();
        return task.ContinueWith(t => {
                if (t.IsCanceled) {
                    res.TrySetCanceled();
                }
                else if (t.IsFaulted) {
                    res.TrySetException(t.Exception!.InnerExceptions[0]);
                }
                else {
                    var x = t.Result.OrElseThrow();
                    res.TrySetResult((O) x);
                }
                return res.Task;
            },
            TaskContinuationOptions.ExecuteSynchronously).Unwrap();
    }
    
    public static Task<O?> CastNullable<O>(this Task<object?> task) {
        TaskCompletionSource<O?> res = new TaskCompletionSource<O?>();
        return task.ContinueWith(t => {
                if (t.IsCanceled) {
                    res.TrySetCanceled();
                }
                else if (t.IsFaulted) {
                    res.TrySetException(t.Exception!.InnerExceptions[0]);
                }
                else {
                    var x = t.Result;
                    res.TrySetResult((O?) x);
                }
                return res.Task;
            },
            TaskContinuationOptions.ExecuteSynchronously).Unwrap();
    }

    public static Task<object> CastToObject<O>(this Task<O> task) {
        TaskCompletionSource<object> res = new TaskCompletionSource<object>();
        return task.ContinueWith(t => {
                if (t.IsCanceled) {
                    res.TrySetCanceled();
                }
                else if (t.IsFaulted) {
                    res.TrySetException(t.Exception!.InnerExceptions[0]);
                }
                else {
                    var x = t.Result.OrElseThrow();
                    res.TrySetResult(x!);
                }
                return res.Task;
            },
            TaskContinuationOptions.ExecuteSynchronously).Unwrap();
    }
    
    public static Task<object?> CastToObjectNullable<O>(this Task<O> task) {
        TaskCompletionSource<object?> res = new TaskCompletionSource<object?>();
        return task.ContinueWith(t => {
                if (t.IsCanceled) {
                    res.TrySetCanceled();
                }
                else if (t.IsFaulted) {
                    res.TrySetException(t.Exception!.InnerExceptions[0]);
                }
                else {
                    var x = t.Result;
                    res.TrySetResult(x);
                }
                return res.Task;
            },
            TaskContinuationOptions.ExecuteSynchronously).Unwrap();
    }
    
    public static Task<object?> InjectNull(this Task task) {
        TaskCompletionSource<object?> res = new TaskCompletionSource<object?>();
        return task.ContinueWith(t => {
                if (t.IsCanceled) {
                    res.TrySetCanceled();
                }
                else if (t.IsFaulted) {
                    res.TrySetException(t.Exception!);
                }
                else {
                    res.TrySetResult(null);
                }
                return res.Task;
            },
            TaskContinuationOptions.ExecuteSynchronously).Unwrap();
    }

}