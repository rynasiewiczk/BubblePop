using UniRx;

namespace Model.ScrollingRowsDown
{
    public interface IScrollRowsController
    {
        ReactiveCommand<int> RowsScrolled { get; }
    }
}