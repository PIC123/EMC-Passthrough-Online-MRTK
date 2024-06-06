using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class LocationSyncModel
{
    [RealtimeProperty(1, true, true)]
    private double _latitude;
    [RealtimeProperty(2, true, true)]
    private double _longitude;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class LocationSyncModel : RealtimeModel {
    public double latitude {
        get {
            return _latitudeProperty.value;
        }
        set {
            if (_latitudeProperty.value == value) return;
            _latitudeProperty.value = value;
            InvalidateReliableLength();
            FireLatitudeDidChange(value);
        }
    }
    
    public double longitude {
        get {
            return _longitudeProperty.value;
        }
        set {
            if (_longitudeProperty.value == value) return;
            _longitudeProperty.value = value;
            InvalidateReliableLength();
            FireLongitudeDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(LocationSyncModel model, T value);
    public event PropertyChangedHandler<double> latitudeDidChange;
    public event PropertyChangedHandler<double> longitudeDidChange;
    
    public enum PropertyID : uint {
        Latitude = 1,
        Longitude = 2,
    }
    
    #region Properties
    
    private ReliableProperty<double> _latitudeProperty;
    
    private ReliableProperty<double> _longitudeProperty;
    
    #endregion
    
    public LocationSyncModel() : base(null) {
        _latitudeProperty = new ReliableProperty<double>(1, _latitude);
        _longitudeProperty = new ReliableProperty<double>(2, _longitude);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _latitudeProperty.UnsubscribeCallback();
        _longitudeProperty.UnsubscribeCallback();
    }
    
    private void FireLatitudeDidChange(double value) {
        try {
            latitudeDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireLongitudeDidChange(double value) {
        try {
            longitudeDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _latitudeProperty.WriteLength(context);
        length += _longitudeProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _latitudeProperty.Write(stream, context);
        writes |= _longitudeProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.Latitude: {
                    changed = _latitudeProperty.Read(stream, context);
                    if (changed) FireLatitudeDidChange(latitude);
                    break;
                }
                case (uint) PropertyID.Longitude: {
                    changed = _longitudeProperty.Read(stream, context);
                    if (changed) FireLongitudeDidChange(longitude);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _latitude = latitude;
        _longitude = longitude;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
