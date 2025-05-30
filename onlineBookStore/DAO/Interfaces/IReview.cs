﻿using onlineBookStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.DAO.Interfaces
{
    public interface IReviewDAO
    {
        bool AddReview(Review review);
        List<Review> GetReviewsByBookId(int bookId);
    }
}
