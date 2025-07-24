using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.DTO
{
    public enum BorrowStatus
    {
        Reservation,
        Borrowing,
        Canceled,
        Overdue,
        Lost,
        Returned,
    }
}
