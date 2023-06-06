
namespace CG.Tools.QuickCrypto.Pages;

/// <summary>
/// This class is the code-behind for the <see cref="MvvmPageBase{TViewModel}"/> component.
/// </summary>
/// <typeparam name="TViewModel">The associated view-model for the page.</typeparam>
public partial class MvvmPageBase<TViewModel> : ComponentBase, IDisposable
    where TViewModel : ObservableObject
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field indicates whether the class has been disposed.
    /// </summary>
    internal protected bool _isDisposed;

    #endregion

    /// <summary>
    /// This property contains the view-model instance associated with the page.
    /// </summary>
    [Inject]
    public TViewModel ViewModel { get; set; } = null!;

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called by Blazor to initialize the component.
    /// </summary>
    protected override void OnInitialized()
    {
        // Watch for property changes in the associated view-model.
        ViewModel.PropertyChanged += (s, e) =>
        {
            StateHasChanged();
        };

        // Give the base class a chance.
        base.OnInitialized();
    }

    // *******************************************************************

    /// <summary>
    /// Override this method to disposed of managed resources in derived classes.
    /// </summary>
    protected virtual void OnDispose()
    {
        // Cleanup the delegate to prevent memory leaks.
        ViewModel.PropertyChanged -= (s, e) => { };
    }

    #endregion

    // *******************************************************************
    // IDisposable methods.
    // *******************************************************************

    #region IDisposable methods

    /// <summary>
    /// This method is called by Blazor to dispose of managed resources.
    /// </summary>
    void IDisposable.Dispose()
    {
        // Are we not yet disposed?
        if (!_isDisposed)
        {
            // Dispose of the object.
            OnDispose();

            // We are disposed.
            _isDisposed = true;
        }
    }

    #endregion
}
