using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryManagementWpf.DTO;
using LibraryManagementWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementWpf.View.Librarian
{
    public partial class FineManagement : UserControl
    {
        private const int ItemsPerPage = 17;
        private int currentPage = 1;
        private int totalItems;

        public ObservableCollection<FineDTO> PaginatedFines { get; set; }
        public List<FineDTO> fines;
        public FineDTO selectedFine;
        public int Finecounts;

        public FineManagement()
        {
            InitializeComponent();
            LoadFines();
        }

        private void LoadFines()
        {
            using (var db = new LibraryManageSystemContext())
            {
                fines = db.Fines
                          .Include(f => f.Borrow.User)
                          .OrderBy(f => f.PaidDate.HasValue)
                          .ThenByDescending(f => f.FineId)
                          .Select(f => new FineDTO
                          {
                              FineId = f.FineId,
                              BorrowId = f.BorrowId ?? 0,
                              BorrowerName = f.Borrow.User.Fullname,
                              FineType = f.FineType ?? "Unknown",
							  FineAmount = f.FineAmount.HasValue ? f.FineAmount.Value.ToString("C", new CultureInfo("vi-VN")) : "0",
							  StatusOfFine = f.Status == 1 ? "Paid" : "Unpaid",
                              PaidDate = f.PaidDate
                          }).ToList();
            }
            totalItems = fines.Count;
            txtNumberOfFines.Text = $"Number of Fines: {totalItems}";
            UpdateFineList();
        }

        private void UpdateFineList()
        {
            var paginatedData = fines
                .Skip((currentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();

            lvFine.ItemsSource = paginatedData;
            UpdatePaginationControls();
        }

        private void UpdatePaginationControls()
        {
            btnPrevious.IsEnabled = currentPage > 1;
            btnNext.IsEnabled = currentPage < TotalPages();
            lblPageInfo.Text = $"Page {currentPage} of {TotalPages()}";
        }

        private int TotalPages()
        {
            return (int)Math.Ceiling((double)totalItems / ItemsPerPage);
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < TotalPages())
            {
                currentPage++;
                UpdateFineList();
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdateFineList();
            }
        }
        private void lvFine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedFine = lvFine.SelectedItem as FineDTO;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();

            var filteredFines = fines.Where(f =>
                f.BorrowerName.ToLower().Contains(searchText) ||
                f.FineType.ToLower().Contains(searchText) ||
                f.FineAmount.ToString().Contains(searchText)).ToList();

            lvFine.ItemsSource = filteredFines;
        }

        private void cbSortOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSortOptions.SelectedItem is ComboBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Highest Fine":
                        lvFine.ItemsSource = fines.OrderByDescending(b => b.FineAmount).ToList();
                        break;
                    case "Lowest Fine":
                        lvFine.ItemsSource = fines.OrderBy(b => b.FineAmount).ToList();
                        break;
                    case "By Borrower Name":
                        lvFine.ItemsSource = fines.OrderBy(b => b.BorrowerName).ToList();
                        break;
                    case "By Fine ID":
                        lvFine.ItemsSource = fines.OrderBy(b => b.FineId).ToList();
                        break;
                }
            }
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
			var result = MessageBox.Show(
					$"Are you sure you want to update status ?",
					"Update Status Borrow",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question
				);
            if (result == MessageBoxResult.Yes)
            {
                if (sender is Button button && button.CommandParameter is FineDTO fine)
                {
                    if (fine.StatusOfFine == "Unpaid")
                    {
                        using (var db = new LibraryManageSystemContext())
                        {
                            var fineToUpdate = db.Fines.Find(fine.FineId);
                            if (fineToUpdate != null)
                            {
                                fineToUpdate.Status = 1;
                                fineToUpdate.PaidDate = DateTime.Now;
                                db.SaveChanges();
                            }
                        }

                        fine.StatusOfFine = "Paid";
                        fine.PaidDate = DateTime.Now;

                        UpdateFineList();
                    }
                }
            }
        }

    }
}
