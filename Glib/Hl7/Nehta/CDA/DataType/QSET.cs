using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public abstract class QSET<T> : ICollection<T> where T : QTY
  {
    //inner ArrayList object
    protected ArrayList _innerArray;
    
    // Default constructor
    public QSET()
    {
      _innerArray = new ArrayList();
    }

    // Default accessor for the collection 
    public virtual T this[int index]
    {
      get
      {
        return (T)_innerArray[index];
      }
      set
      {
        _innerArray[index] = value;
      }
    }

    // Number of elements in the collection
    public virtual int Count
    {
      get
      {
        return _innerArray.Count;
      }
    }

    //flag for setting collection to read-only mode
    protected bool _IsReadOnly = false;
    public virtual bool IsReadOnly
    {
      get
      {
        return _IsReadOnly;
      }
    }

    // Add a business object to the collection
    public virtual void Add(T item)
    {
      _innerArray.Add(item);
    }

    // Remove first instance of a business object from the collection 
    public virtual bool Remove(T item)
    {
      bool result = false;

      //loop through the inner array's indices
      for (int i = 0; i < _innerArray.Count; i++)
      {
        //store current index being checked
        T obj = (T)_innerArray[i];

        //compare the BusinessObjectBase UniqueId property
        if (obj.UniqueId == item.UniqueId)
        {
          //remove item from inner ArrayList at index i
          _innerArray.RemoveAt(i);
          result = true;
          break;
        }
      }

      return result;
    }

    // Returns true/false based on whether or not it finds
    // the requested object in the collection.
    public bool Contains(T item)
    {
      //loop through the inner ArrayList
      foreach (T obj in _innerArray)
      {
        //compare the BusinessObjectBase UniqueId property
        if (obj.UniqueId == item.UniqueId)
        {
          //if it matches return true
          return true;
        }
      }
      //no match
      return false;
    }

    // Copy objects from this collection into another array
    public virtual void CopyTo(T[] item, int index)
    {
      _innerArray.CopyTo(item, index);      
    }

    // Clear the collection of all it's elements
    public virtual void Clear()
    {
      _innerArray.Clear();
    }

    // Returns custom generic enumerator for this QSET Collection
    public virtual IEnumerator<T> GetEnumerator()
    {
      //return a custom enumerator object instantiated
      //to use this QSET Collection 
      return new QSETEnumerator<T>(this);
    }

    // Explicit non-generic interface implementation for IEnumerable
    // extended and required by ICollection (implemented by ICollection<T>)
    IEnumerator IEnumerable.GetEnumerator()
    {
      return new QSETEnumerator<T>(this);
    }
  }


  /// <summary>
  /// Class required to implement IEnumerator to allow foreach on QSET collection
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class QSETEnumerator<T> : IEnumerator<T> where T : QTY
  {
    protected QSET<T> _collection; //enumerated collection
    protected int index; //current index
    protected T _current; //current enumerated object in the collection

    // Default constructor
    public QSETEnumerator()
    {
      //nothing
    }

    // Paramaterized constructor which takes
    // the collection which this enumerator will enumerate
    public QSETEnumerator(QSET<T> collection)
    {
      _collection = collection;
      index = -1;
      _current = default(T);
    }

    // Current Enumerated object in the inner collection
    public virtual T Current
    {
      get
      {
        return _current;
      }
    }

    // Explicit non-generic interface implementation for IEnumerator
    // (extended and required by IEnumerator<T>)
    object IEnumerator.Current
    {
      get
      {
        return _current;
      }
    }

    // Dispose method
    public virtual void Dispose()
    {
      _collection = null;
      _current = default(T);
      index = -1;
    }

    // Move to next element in the inner collection
    public virtual bool MoveNext()
    {
      //make sure we are within the bounds of the collection
      if (++index >= _collection.Count)
      {
        //if not return false
        return false;
      }
      else
      {
        //if we are, then set the current element
        //to the next object in the collection
        _current = _collection[index];
      }
      //return true
      return true;
    }

    // Reset the enumerator
    public virtual void Reset()
    {
      _current = default(T); //reset current object
      index = -1;
    }
  }

}
