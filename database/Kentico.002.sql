IF NOT EXISTS (SELECT * FROM OM_ActivityType WHERE ActivityTypeName = 'UCommerceProductAddedToBasket')
BEGIN
	INSERT INTO OM_ActivityType
	(
		ActivityTypeDisplayName,
		ActivityTypeName,
		ActivityTypeEnabled,
		ActivityTypeIsCustom,
		ActivityTypeDescription,
		ActivityTypeManualCreationAllowed,
		ActivityTypeMainFormControl,
		ActivityTypeDetailFormControl,
		ActivityTypeIsHiddenInContentOnly
	)
	VALUES
	(
		'Ucommerce Product added to basket',
		'UCommerceProductAddedToBasket',
		1,
		0,
		'The visitor added a product to the Ucommerce basket',
		0,
		'UcommerceProductIdSelector',
		NULL,
		0
	)
END

IF NOT EXISTS (SELECT * FROM OM_ActivityType WHERE ActivityTypeName = 'UCommerceProductPurchased')
BEGIN
	INSERT INTO OM_ActivityType
	(
		ActivityTypeDisplayName,
		ActivityTypeName,
		ActivityTypeEnabled,
		ActivityTypeIsCustom,
		ActivityTypeDescription,
		ActivityTypeManualCreationAllowed,
		ActivityTypeMainFormControl,
		ActivityTypeDetailFormControl,
		ActivityTypeIsHiddenInContentOnly
	)
	VALUES
	(
		'Ucommerce Product purchased',
		'UCommerceProductPurchased',
		1,
		0,
		'The visitor purchased a product',
		0,
		'UcommerceProductIdSelector',
		NULL,
		0
	)
END

IF NOT EXISTS (SELECT * FROM OM_ActivityType WHERE ActivityTypeName = 'UCommercePurchaseMade')
BEGIN
	INSERT INTO OM_ActivityType
	(
		ActivityTypeDisplayName,
		ActivityTypeName,
		ActivityTypeEnabled,
		ActivityTypeIsCustom,
		ActivityTypeDescription,
		ActivityTypeManualCreationAllowed,
		ActivityTypeMainFormControl,
		ActivityTypeDetailFormControl,
		ActivityTypeIsHiddenInContentOnly
	)
	VALUES
	(
		'Ucommerce Purchase made',
		'UCommercePurchaseMade',
		1,
		0,
		'The visitor made a purchase',
		0,
		NULL,
		NULL,
		0
	)
END

IF NOT EXISTS (SELECT * FROM OM_ActivityType WHERE ActivityTypeName = 'UCommerceBasketAbandoned')
BEGIN
	INSERT INTO OM_ActivityType
	(
		ActivityTypeDisplayName,
		ActivityTypeName,
		ActivityTypeEnabled,
		ActivityTypeIsCustom,
		ActivityTypeDescription,
		ActivityTypeManualCreationAllowed,
		ActivityTypeMainFormControl,
		ActivityTypeDetailFormControl,
		ActivityTypeIsHiddenInContentOnly
	)
	VALUES
	(
		'Ucommerce Basket abandoned',
		'UCommerceBasketAbandoned',
		1,
		0,
		'The visitor abandoned a basket',
		0,
		NULL,
		NULL,
		0
	)
END

IF NOT EXISTS (SELECT * FROM OM_ActivityType WHERE ActivityTypeName = 'UCommerceProductRemovedFromBasket')
BEGIN
	INSERT INTO OM_ActivityType
	(
		ActivityTypeDisplayName,
		ActivityTypeName,
		ActivityTypeEnabled,
		ActivityTypeIsCustom,
		ActivityTypeDescription,
		ActivityTypeManualCreationAllowed,
		ActivityTypeMainFormControl,
		ActivityTypeDetailFormControl,
		ActivityTypeIsHiddenInContentOnly
	)
	VALUES
	(
		'Ucommerce Product removed from basket',
		'UCommerceProductRemovedFromBasket',
		1,
		0,
		'The visitor removed a product from the Ucommerce basket',
		0,
		'UcommerceProductIdSelector',
		NULL,
		0
	)
END