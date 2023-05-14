Feature: Create comment

Scenario: Create comment for defect
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    And I have opened defects page
    When I write a comment and submit
    Then new comment should be displayed