using LibraryManagementWpf.EmailSender;
using LibraryManagementWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace LibraryManagementWpf.Services
{


    public class CheckerService
    {
        private readonly System.Timers.Timer _timer;
        private readonly LibraryManageSystemContext _context;
        private readonly EmailSender.EmailSender _es;

        public CheckerService(LibraryManageSystemContext context, EmailSender.EmailSender es)
        {
            _context = context;
            _es = es;

            var currentTime = DateTime.Now;
            var nextRunTime = currentTime.Date.AddDays(1).AddMinutes(1);
            var initialDelay = nextRunTime - currentTime;

            _timer = new System.Timers.Timer(initialDelay.TotalMilliseconds);
            /*			_timer = new System.Timers.Timer(60000);
            */
            _timer.Elapsed += CheckFinesAndUpdate;
            _timer.AutoReset = false;
            _timer.Start();
        }
        private async void CheckFinesAndUpdate(object sender, ElapsedEventArgs e)
        {
            var overdueBooks = _context.BorrowBooks.Include(b => b.Fines).Include(b => b.User)
                    .Where(b => b.DueDate.HasValue && b.Status == 1)
                    .ToList() 
                    .Where(b => (DateTime.Now - b.DueDate.Value).Days == 1)
                    .Where(b => !b.Fines.Any())
                    .ToList();  
            foreach (var b in overdueBooks)
            {
                var fine = new Fine
                {
                    FineType = "Overdue 1 day",
                    FineAmount = 10000,
                    Status = 0,
                    BorrowId = b.BorrowId,
                    PaidDate = null
                };
                _context.Fines.Add(fine);
                _context.SaveChanges();
                await _es.SendEmail(b.User.Email,"Fine Notify","fineNotify", b.BorrowId);
            }

            var fines = _context.Fines.Include(f => f.Borrow)
                .Where(f => f.Status == 0 && f.Borrow.Status == 1)
                .ToList();

            foreach (var f in fines)
            {
                var dueDate = f.Borrow.DueDate.Value;
                var overdueDays = (DateTime.Now - dueDate).Days;

                if (overdueDays > 0)
                {
                    f.FineType = $"Overdue {overdueDays} days";
                    f.FineAmount = overdueDays * 10000;
                }

                if(overdueDays == 3)
                {
                    await _es.SendEmail(f.Borrow.User.Email, "Fine Notify", "fineNotify", f.Borrow.BorrowId);
                }

                if (overdueDays == 5)
                {
                    await _es.SendEmail(f.Borrow.User.Email, "Fine Notify", "fineNotify", f.Borrow.BorrowId);
                }
            }

            await _context.SaveChangesAsync();
        }

        public void StopService()
        {
            _timer.Stop();
        }

    }
}
