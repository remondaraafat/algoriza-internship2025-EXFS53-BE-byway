namespace APICoursePlatform.Enums
{
    public class Enums
    {
        public enum UserRole
        {
            User,
            Admin
        }

        public enum JobTitle
        {
            FullstackDeveloper,
            BackendDeveloper,
            FrontendDeveloper,
            UXUIDesigner
        }

        public enum CourseLevel
        {
            Beginner,
            Intermediate,
            Expert,
            AllLevels
        }
        //public enum NotificationType
        //{
        //    General,
        //    NewOffer,
        //    OfferAccepted,
        //    NewMessage,
        //    PaymentReceived,
        //    ShipmentStatusChanged,
        //    ShipmentDelivered,
        //    RatingReceived
        //}
        public enum CourseSortBy
        {
            Latest = 1,
            Oldest = 2,
            HighestPrice = 3,
            LowestPrice = 4
        }
    }
}
