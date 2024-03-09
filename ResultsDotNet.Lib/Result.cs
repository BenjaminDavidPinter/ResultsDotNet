namespace BensWorkbench.Models;

/// <summary>
/// Class which encapsulates function results as either a success type <typeparamref name="T"/>, 
/// or some error type which extends Exception <typeparamref name="E"/>
/// </summary>
/// <typeparam name="T">Type for object which represents success</typeparam>
public class Result<T> : IEquatable<T>
{

    /// <summary>
    /// </summary>
    /// Backing store for Exception object
    private Exception? ErrorObject { get; }

    /// <summary>
    /// Backing store for the success value object
    /// </summary>
    private T? ValueInternal { get; }

    /// <summary>
    /// Constructor used when returning a successful value from any function which returns a Result<typeparamref name="T"/>>
    /// </summary>
    /// <param name="obj"></param>
    public Result(T obj)
    {
        ValueInternal = obj;
    }

    /// <summary>
    /// Constructor used when returning an error from any function which returns a Reuslt<typeparamref name="T"/>>
    /// </summary>
    /// <param name="exception"></param>
    public Result(Exception exception)
    {
        ErrorObject = exception;
    }

    /// <summary>
    /// Shortcut to get the success value object from Result<typeparamref name="T"/>>
    /// </summary>
    /// <returns></returns>
    public T Unwrap()
    {
        if (ErrorObject is not null)
            throw ErrorObject;


        return ValueInternal ??
          throw (ErrorObject ?? new Exception("Attempted to Unwrap an unused Result"));
    }

    /// <summary>
    /// Check to see if the result is error
    /// </summary>
    /// <returns></returns>
    public bool IsErr()
    {
        return ErrorObject is not null;
    }

    /// <summary>
    /// Check to see if the result is an exception of type <typeparamref name="F"/>
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <returns></returns>
    public bool IsErr<F>() where F : Exception
    {
        if (ErrorObject is null)
            return false;

        return typeof(F).Equals(ErrorObject!.GetType());
    }

    /// <summary>
    /// Check to see if the result is a success value
    /// </summary>
    /// <returns></returns>
    public bool IsOK()
    {
        return ErrorObject is null && ValueInternal is not null;
    }

    /// <summary>
    /// For casting from <typeparamref name="T"/> object to Result<typeparamref name="T"/>>
    /// </summary>
    /// <param name="obj"></param>
    public static implicit operator Result<T>(T obj) => new(obj);

    /// <summary>
    /// For casting from any Exception to Result<typeparamref name="T"/>>
    /// </summary>
    /// <param name="exception"></param>
    public static implicit operator Result<T>(Exception exception) => new(exception);

    /// <summary>
    /// For assigning the value of Result<typeparamref name="T"/>>, when successful to an object of type <typeparamref name="T"/>
    /// </summary>
    /// <param name="rslt"></param>
    public static implicit operator T(Result<T> rslt) => rslt.Unwrap();

    /// <summary>
    /// Allow for direct comparisons between the success object, and an object of the same type
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static bool operator ==(Result<T> obj1, T obj2)
    {
        return obj1.Equals(obj2);
    }

    /// <summary>
    /// Allow for direct comparisons between the success object, and an object of the same type
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static bool operator !=(Result<T> obj1, T obj2)
    {
        return obj1.Equals(obj2);
    }

    /// <summary>
    /// Allow for direct comparisons between the internal error, and an Exception
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static bool operator ==(Result<T> obj1, Exception obj2)
    {
        try
        {
            obj1.Unwrap();
            return false;
        }
        catch (Exception exception)
        {
            return exception.Message == obj2.Message;
        }
    }

    /// <summary>
    /// Allow for direct comparisons between the internal error, and an Exception
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static bool operator !=(Result<T> obj1, Exception obj2)
    {
        try
        {
            obj1.Unwrap();
            return true;
        }
        catch (Exception exception)
        {
            return exception.Message != obj2.Message; ;
        }
    }

    /// <summary>
    /// To satisfy Interface
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(T? other)
    {
        return other?.Equals(ValueInternal) ?? false;
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
