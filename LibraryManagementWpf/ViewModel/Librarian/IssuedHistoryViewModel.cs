using LibraryManagementWpf.DTO;
using LibraryManagementWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.ViewModel.Librarian
{
    public class IssuedHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<BorrowBookDTO> BorrowBooks { get; set; }
        private int issueCount;
        public int IssueCount
        {
            get => issueCount;
            private set
            {
                issueCount = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> StatusOptions { get; set; }
        private string selectedBorrowStatus;
        public string SelectedBorrowStatus
        {
            get => selectedBorrowStatus;
            set
            {
                selectedBorrowStatus = value;
                OnPropertyChanged();
                LoadBorrowBooks();
            }
        }

        public ObservableCollection<string> SortOptions { get; set; }
        private string selectedSortOption;
        public string SelectedSortOption
        {
            get => selectedSortOption;
            set
            {
                selectedSortOption = value;
                OnPropertyChanged();
                LoadBorrowBooks();
            }
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                LoadBorrowBooks();
            }
        }

        public IssuedHistoryViewModel()
        {
            BorrowBooks = new ObservableCollection<BorrowBookDTO>();
            StatusOptions = new ObservableCollection<string>
            {
                "All",
                "Returned",
                "Cancled",
            };
            SortOptions = new ObservableCollection<string>
            {
                "Oldest",
                "Newest"
            };
            LoadBorrowBooks();

            SelectedSortOption = SortOptions.First();
            SelectedBorrowStatus = StatusOptions.First();
        }

        public void LoadBorrowBooks()
        {
            using (var db = new LibraryManageSystemContext())
            {
                var query = db.BorrowBooks
                    .Include(b => b.Book)
                    .Include(b => b.User)
                    .Where(b => b.Status == (int)BorrowStatus.Returned || b.Status == (int)BorrowStatus.Canceled)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    query = query.Where(b => b.User.Fullname.Contains(SearchText) ||
                                            b.User.StudentCode.Contains(SearchText) ||
                                            b.Book.Title.Contains(SearchText));
                }

                if (SelectedBorrowStatus != "All")
                {
                    int statusFilter = SelectedBorrowStatus == "Returned" ? (int)BorrowStatus.Returned : (int)BorrowStatus.Canceled;
                    query = query.Where(b => b.Status == statusFilter);
                }

                switch (SelectedSortOption)
                {
                    case "Oldest":
                        query = query.OrderBy(b => b.ReturnDate);
                        break;
                    case "Newest":
                        query = query.OrderByDescending(b => b.ReturnDate);
                        break;
                    default:
                        break;
                }

                var borrowBooksFromDb = query.ToList();
                BorrowBooks.Clear();
                foreach (var borrowBook in borrowBooksFromDb)
                {
                    BorrowBooks.Add(new BorrowBookDTO
                    {
                        BorrowId = borrowBook.BorrowId,
                        StudentCode = borrowBook.User.StudentCode,
                        BorrowerName = borrowBook.User.Fullname,
                        BookTitle = borrowBook.Book.Title,
                        ReservationDate = borrowBook.ReservationDate,
                        BorrowDate = borrowBook.BorrowDate,
                        DueDate = borrowBook.DueDate,
                        ReturnDate = borrowBook.ReturnDate,
                        LibrarianInCharge = borrowBook.LibrarianInCharge,
                        Status = (BorrowStatus)borrowBook.Status
                    });
                }
                IssueCount = BorrowBooks.Count;
            }
        }

    }
}
