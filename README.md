[![Nuget](https://img.shields.io/nuget/v/Xam.Plugin.Once)](https://www.nuget.org/packages/Xam.Plugin.Once) ![Nuget](https://img.shields.io/nuget/dt/Xam.Plugin.Once)

![Icon](https://raw.githubusercontent.com/galadril/Xam.Plugin.Once/master/Samples/Xam.Plugin.Once.Samples/Xam.Plugin.Once.Samples.Android/Resources/mipmap-xxhdpi/ic_launcher.png)

# Xam.Plugin.Once
Just a nice and library that allows you to do something once for your Xamarin Forms project.

!!This was inspired by the Once library for Android made by jonfinerty!!
https://github.com/jonfinerty/Once

----

Some things should happen **once**.
* Users should only get the guided tour _once_. 
* Release notes should only pop up _once every app upgrade_. 
* Your app should only phone home to update content _once every hour_.

`Once` provides a simple API to track whether or not your app has already performed an action within a given scope.



# Setup
* Available on Nuget:
https://www.nuget.org/packages/Xam.Plugin.Once

!!Install into your .net standard Forms project. !!


# Usage
Here are some examples on how to use Once for Xamarin Forms:

- Only run the app intro once
 
```
            Once.Instance.RunWhen(SHOW_APP_INTRO, new Command(() => UserDialogs.Instance.Toast("Show app intro")));
 
```


- Only show a survey every 30 days for the first time, then after 90 days
 
```
            if(Once.Instance.LastRunAt(SHOW_SURVEY) == null)
                Once.Instance.RunWhen(SHOW_SURVEY, new Command(() => UserDialogs.Instance.Toast("Show app survey")), new After() {RunAfter = 30, Type = After.AfterType.Days });
            else
                Once.Instance.RunWhen(SHOW_SURVEY, new Command(() => UserDialogs.Instance.Toast("Show app survey")), new After() {RunAfter = 90, Type = After.AfterType.Days });
 
```

- // just to check if we need to show the app intro
 
```
            _ = Once.Instance.NeedsToRun(SHOW_APP_INTRO);
 
```

- // Mark a task to done
 
```
            Once.Instance.MarkRunAsDone(SHOW_APP_CHANGELOG);
 
```


(see sample project for more info)
