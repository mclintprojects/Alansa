# Alansa
A collection of handy classes I use in my freelance projects now easily accessible in one place.

### Android

#### App
Ideally your `[Application]` class should inherit the `App` class defined in `Alansa.Droid`. Some other parts of the library will not work
otherwise.

If your application requires MultiDex, set `MultiDex enabled` to true in the project properties and inherit the `MultiDexApplication` class
defined in `Alansa.Droid`.

#### Resources
Alansa.Droid comes with a few resources [styles, colors, layout files] to speed up your work and prevent you from doing the same thing over
and over again.

##### Styles
Alansa.Droid has a base style `Theme.Alansa.BaseTheme` that inherit from `Theme.AppCompat.Light.` It also has two other styles for a Splashscreen
`Theme.Alansa.AppFullscreen` and a normal app theme `Theme.Alansa.AppTheme`.

###### Setting your custom colors for the normal app theme
```xml
<style name="AppTheme" parent="Theme.Alansa.AppTheme">
  <item name="colorPrimary">YOUR_PRIMARY_COLOR_HERE</item>
  <item name="colorPrimaryDark">YOUR_PRIMARY_DARK_COLOR_HERE</item>
  <item name="colorAccent">YOUR_ACCENT_COLOR_HERE</item>
 </style>
  ```
  
###### Setting your custom image for your splashscreen
Create a splashscreen drawable, ideally named `splashscreen.xml`. Put it in `Resources\drawable`.
Here's a template of it's content:
```xml
<?xml version="1.0" encoding="utf-8"?>
<layer-list xmlns:android="http://schemas.android.com/apk/res/android">
  <item android:drawable="@color/YOUR_BACKGROUND_COLOR_HERE"/>
  <item> <bitmap android:src="@drawable/YOUR_LOGO_DRAWABLE_HERE" android:gravity="center" /> </item>
</layer-list>
```

###### Setting your custom splashscreen in your splashscreen theme
```xml
<style name="SplashscreenTheme" parent="Theme.Alansa.AppFullscreen">
  <item name="android:windowBackground">@drawable/YOUR_SPLASHSCREEN_DRAWABLE_NAME_HERE</item>
</style>
```

##### Colors
Alansa.Droid provides you with various colors.
1. Material design primary text color BLACK `@color/primaryText`
2. Material design secondary text color BLACK `@color/primaryTextLight`
3. Material design primary text color WHITE `@color/secondaryText`
4. Material design secondary text color WHITE `@color/secondaryTextLight`
5. White `@color/white`
6. Window background color `@color/windowBg`

##### Layout
Alansa.Droid comes with a Toolbar layout `@layout/toolbar` already provided. Ideally you should include it in the layout file of any
Activity you want to have a toolbar. `Alansa.Droid.Activities.BaseActivity` already handles inflating and setting up the toolbar for you
but you'd need to make sure that the toolbar has an id of `@+id/toolbar` otherwise inflation would fail.
