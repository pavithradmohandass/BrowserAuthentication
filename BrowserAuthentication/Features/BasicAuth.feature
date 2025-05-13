Feature: HTTP Basic Auth via CDP

  Scenario: User logs in with valid credentials
    Given I authenticate using CDP
    Then I should see the success message