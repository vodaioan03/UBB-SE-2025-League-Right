using Microsoft.UI.Xaml.Data;
using System;

namespace Duo.Converters
{
    public class QuizCommandParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int quizId && parameter is bool isExam)
            {
                return (quizId, isExam);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
