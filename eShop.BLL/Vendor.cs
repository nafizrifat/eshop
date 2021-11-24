﻿using eShop.Common;
using System;

namespace eShop.BLL
{
    /// <summary>
    /// Manages the vendors from whom we purchase our inventory.
    /// </summary>
    public class Vendor
    {
        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Sends a product order to vendor
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public OperationResult PlaceOrder(Product product, int quality)
        {
            return PlaceOrder(product, quality, null, null);
        }

        /// <summary>
        /// Sends a product order to vendor
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quality"></param>
        /// <param name="deliveryBy"></param>
        /// <returns></returns>
        public OperationResult PlaceOrder(Product product, int quality, DateTimeOffset? deliveryBy)
        {
            return PlaceOrder(product, quality, deliveryBy, null);
        }

        /// <summary>
        /// Sends a product order to vendor
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quality"></param>
        /// <param name="deliveryBy"></param>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public OperationResult PlaceOrder(Product product, int quality, DateTimeOffset? deliveryBy, String instructions)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (quality <= 0)
                throw new ArgumentOutOfRangeException(nameof(quality));
            if (deliveryBy <= DateTimeOffset.Now)
                throw new ArgumentOutOfRangeException(nameof(deliveryBy));

            bool success = false;

            var orderText = "Order from eShop" + System.Environment.NewLine +
                            "Product:" + product.ProductCode + System.Environment.NewLine +
                            "Quantity:" + quality;

            if (deliveryBy.HasValue)
            {
                orderText += System.Environment.NewLine + "Deliver By:" + deliveryBy.Value.ToString("d");
            }
            if (!string.IsNullOrWhiteSpace(instructions))
            {
                orderText += System.Environment.NewLine + "Instructions:" + instructions;
            }

            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Order", orderText, this.Email);
            if (confirmation.StartsWith("Message Sent"))
            {
                success = true;
            }

            var operationResult = new OperationResult(success, orderText);
            return operationResult;
        }

        /// <summary>
        /// Sends an email to welcome a new vendor.
        /// </summary>
        /// <returns></returns>
        public string SendWelcomeEmail(string message) 
        {
            var emailService = new EmailService();
            var subject = ("Hello " + this.CompanyName).Trim();
            var confirmation = emailService.SendMessage(subject,
                                                        message,
                                                        this.Email);
            return confirmation;
        }

        /// <summary>
        /// Placing the order 
        /// </summary>
        /// <param name="product">Product to order</param>
        /// <param name="quality">Quantity of the product to order</param>
        /// <param name="includeAddress">TRUE when shipping address included</param>
        /// <param name="sendCopy">TRUE when send copy of email</param>
        /// <returns>Success Flag and order text</returns>
        public OperationResult PlaceOrder(Product product, int quality, bool includeAddress, bool sendCopy)
        {
            var orderText = "Test";
            if (includeAddress) orderText += " With Address";
            if (sendCopy) orderText += "With Copy";

            var operationResult = new OperationResult(true, orderText);
            return operationResult;
        }
    }
}