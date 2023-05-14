Feature: Change defect status

Scenario: Change defect status when it is unfixed
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    And I have opened defects page
    When I change defect status
    Then defect status change activity is displayed