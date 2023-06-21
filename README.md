# subtrack <em>- A simplistic subscription tracker</em>

Keep track of what subscriptions you have, when they have to be paid and how much you are paying.

# Table of contents
- [Development info](#development-info)
- [Current WIP](#setup---chrisk00---wip)
- [MVP](#mvp)
- [Roadmap](#roadmap-2023)

# Development Info

## Running the project
The project currently does not require any configuration at all. On startup data is seeded to a locally stored sqlite database.

1. To begin with we only have a mobile app and we focus on making the design and functionality work for Android. You do not need an android phone to actually run the project but you do need to enable **Hyper-V** on your PC https://learn.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/hardware-acceleration?pivots=windows.
2.  After enabling Hyper-V you can run the project with the **Android emulator** - I am using a **Pixel 5 - API 31 (Android 12.0)** since that was default. You can add the device by opening the **Android Device Manager** inside VS

*Note that you can run the project without the android emulator as long as you won't be making any UI changes, alternatively tell someone else to verify the UI looks ok*

## Github

1. Clone the project and checkout the dev branch.
2. Assign yourself to an issue and create a new branch from dev with this format: **$"feature/{InsertTitleOfIssue}"**
3. Make some changes, commit the changes then push them to github
4. Open up a Pull request from the branch with the changes to the dev branch
5. Wait for other devs to give feedback. When the Pull request has been approved click squash merge and delete the branch afterwards

## Creating database migrations

After making changes to any entity you need to add a migration and update the database

1. Open up the **Package manager console** found in **View->Other Windows**
2. Change the "Default Project:" to "subtrack.DAL"
3. Enter this command in the console `add-migration {insertMigrationName} -StartupProject subtrack.DAL`
4. Make sure the generated files are as expected. If you wish to undo the migration write `remove-migration -StartupProject subtrack.DAL`
5. Enter this command in the console `update-database {insertMigrationName} -StartupProject subtrack.DAL`

# Setup - @chrisk00 - WIP

- [x] Blazor MAUI + EF + SQLite
- [x] add UI mockups
- [x] Readme Building/running/adding migrations
- [ ] Create issues for MVP.1 and add features to roadmap
- [ ] Create issues for MVP.2 and add features to roadmap
- [ ] Readme Roadmap

## MVP

### MVP.1 - Viewing Subscriptions

#### Start page

- [ ] Display total subscriptions cost per month at the top of the page
- [ ] List every subscription with their name and cost
- [ ] Show how many days left each subscription is due

#### Subscriptions page

- [ ] Display every month and the total cost of subscriptions for that month
- [ ] List subscriptions under each month showing the name, cost, if it's auto paid and how many days left until it's due

### MVP.2 - Creating Subscriptions

## Roadmap 2023
