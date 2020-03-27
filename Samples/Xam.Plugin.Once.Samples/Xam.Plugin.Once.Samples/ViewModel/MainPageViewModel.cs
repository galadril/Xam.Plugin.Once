using Acr.UserDialogs;
using Xam.Plugin.Once.Samples.Helpers;
using Xamarin.Forms;

namespace Xam.Plugin.Once.Samples.ViewModel
{
    /// <summary>
    /// View model to demonstrate <see cref="Once"/>
    /// </summary>
    public class MainPageViewModel : ObservableObject
    {
        #region Constants

        public const string SHOW_APP_INTRO = "showappintro";
        public const string SHOW_APP_CHANGELOG = "showappchangelog";
        public const string SHOW_SURVEY = "showsurvey";

        #endregion


        #region Constructor & Destructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainPageViewModel()
        {
            // Only run the app intro once
            Once.Instance.RunWhen(SHOW_APP_INTRO, new Command(() => UserDialogs.Instance.Toast("Show app intro")));

            // Only show a survey every 30 days for the first time, then after 90 days
            if(Once.Instance.LastRunAt(SHOW_SURVEY) == null)
                Once.Instance.RunWhen(SHOW_SURVEY, new Command(() => UserDialogs.Instance.Toast("Show app survey")), new After() { RunAfter = 30, Type = After.AfterType.Days });
            else
                Once.Instance.RunWhen(SHOW_SURVEY, new Command(() => UserDialogs.Instance.Toast("Show app survey")), new After() { RunAfter = 90, Type = After.AfterType.Days });

            _ = Once.Instance.NeedsToRun(SHOW_APP_INTRO); // just to check if we need to show the app intro
            _ = Once.Instance.NeedsToRun(SHOW_SURVEY, new After() {RunAfter = 30, Type = After.AfterType.Days }); // just to check if we need to show the app survey

            // Mark a task to done
            Once.Instance.MarkRunAsDone(SHOW_APP_CHANGELOG);
        }

        #endregion
    }
}

