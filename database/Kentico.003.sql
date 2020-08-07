DECLARE @id INT
SET @id=18

;WITH hierarchy AS (
	SELECT sc.CategoryID, sc.CategoryParentID, sc.CategoryDisplayName FROM CMS_SettingsCategory sc
		WHERE
			sc.CategoryName = 'CMS.Ecommerce' or
			sc.CategoryName = 'CMS.OnlineMarketing.Activities.ECommerce'
	UNION ALL
	SELECT sc2.CategoryID, sc2.CategoryParentID, sc2.CategoryDisplayName FROM CMS_SettingsCategory sc2 JOIN hierarchy h ON h.CategoryID = sc2.CategoryParentID AND h.CategoryID != sc2.CategoryID
	)

UPDATE CMS_SettingsKey SET KeyIsHidden = '1' WHERE KeyCategoryID IN (SELECT h.CategoryID FROM hierarchy h)
